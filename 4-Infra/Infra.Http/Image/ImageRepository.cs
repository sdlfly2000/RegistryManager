using Core.Options;
using Domain.Image;
using Domain.Image.Repositories;
using Microsoft.Extensions.Options;
using System.Net.Http;
using static System.Net.Mime.MediaTypeNames;

namespace Infra.Http.Image
{
    public class ImageRepository : IImageRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly RegistryOption _option;

        public ImageRepository(IHttpClientFactory httpClientFactory, IOptions<RegistryOption> option) 
        {
            _httpClientFactory = httpClientFactory;
            _option = option.Value;
        }

        public async Task<IList<RepositoryImage>> LoadFullList(CancellationToken token)
        {
            using var httpClient = CreateHttpClient();

            var relativeUrl = string.Concat("v2/", "_catalog");
            using var response = await httpClient.GetAsync(new Uri(relativeUrl, UriKind.Relative), token).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to load full image list. Status code: {response.StatusCode}");
            }

            return default;
        }

        #region Private Methods

        private HttpClient CreateHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_option.BaseUrl);
            return httpClient;
        }

        #endregion
    }
}
