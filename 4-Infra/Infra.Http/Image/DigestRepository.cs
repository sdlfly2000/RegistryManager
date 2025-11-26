using Common.Core.DependencyInjection;
using Core.AOP.Cache;
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
        private readonly IServiceProvider _serviceProvider;

        public DigestRepository(IHttpClientFactory httpClientFactory, IOptions<RegistryOption> option, IServiceProvider serviceProvider)
        {
            _httpClientFactory = httpClientFactory;
            _option = option.Value;
            _serviceProvider = serviceProvider;
        }

        [Cache(masterKey: nameof(EnumCacheMasterKey.Digest),returnType: typeof(Digest), cachedTypes: [typeof(RepositoryImage), typeof(Tag)])]
        public async Task<IDigest?> Load(IRepositoryImage image, ITag tag, CancellationToken token)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_option.BaseUrl);

            var relativeUrl = string.Concat("v2/", image.Name, "/manifests/", tag.Name);
            using var response = await httpClient.GetAsync(new Uri(relativeUrl, UriKind.Relative), token).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to load tags for image {image.Name}. Status code: {response.StatusCode}");
            }

            return new Digest { Code = response.Headers.GetValues("Docker-Content-Digest").First() };
        }
    }
}
