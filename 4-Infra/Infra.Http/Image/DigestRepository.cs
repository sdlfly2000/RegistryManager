using Common.Core.DependencyInjection;
using Core.Options;
using Domain.Image;
using Domain.Image.Repositories;
using Microsoft.Extensions.Options;

namespace Infra.Http.Image
{
    [ServiceLocate(typeof(IDigestRepositry))]
    public class DigestRepository : IDigestRepositry
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly RegistryOption _option;

        public DigestRepository(IHttpClientFactory httpClientFactory, IOptions<RegistryOption> option)
        {
            _httpClientFactory = httpClientFactory;
            _option = option.Value;
        }

        public async Task<IDigest?> Load(IRepositoryImage image, string tagName, CancellationToken token)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_option.BaseUrl);

            var relativeUrl = string.Concat("v2/", image.Name, "/manifests/", tagName);
            using var response = await httpClient.GetAsync(new Uri(relativeUrl, UriKind.Relative), token).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to load tags for image {image.Name}. Status code: {response.StatusCode}");
            }

            return new Digest { Code = response.Headers.GetValues("Docker-Content-Digest").First() };
        }
    }
}
