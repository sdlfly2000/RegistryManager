using Common.Core.DependencyInjection;
using Core.Options;
using Domain.Image;
using Domain.Image.Repositories;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infra.Http.Image
{
    [ServiceLocate(typeof(ITagRepository))]
    public class TagRepository : ITagRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly RegistryOption _option;

        public TagRepository(IHttpClientFactory httpClientFactory, IOptions<RegistryOption> option)
        {
            _httpClientFactory = httpClientFactory;
            _option = option.Value;
        }

        public async Task<List<ITag>?> Load(IRepositoryImage image)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_option.BaseUrl);

            var relativeUrl = string.Concat("v2/", image.Name, "/tags/list");
            using var response = await httpClient.GetAsync(new Uri(relativeUrl, UriKind.Relative)).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to load tags for image {image.Name}. Status code: {response.StatusCode}");
            }

            var contentTags = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            
            return JsonSerializer.Deserialize<List<ITag>>(contentTags);
        }
    }
}
