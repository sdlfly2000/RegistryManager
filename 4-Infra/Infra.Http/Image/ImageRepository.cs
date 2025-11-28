using Common.Core.DependencyInjection;
using Core.Options;
using Domain.Image;
using Domain.Image.Repositories;
using Infra.Http.Image.model;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infra.Http.Image
{
    [ServiceLocate(typeof(IImageRepository))]
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
            var result = new List<RepositoryImage>();

            using var httpClient = CreateHttpClient();

            var relativeUrl = string.Concat("v2/", "_catalog");
            using var response = await httpClient.GetAsync(new Uri(relativeUrl, UriKind.Relative), token).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to load full image list. Status code: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var imagesModel = JsonSerializer.Deserialize<ImagesModel>(content);

            if (imagesModel != null)
            {
                foreach (var repository in imagesModel.Repositories)
                {
                    result.Add(new RepositoryImage(repository));
                }
            }
            
            return result;
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
