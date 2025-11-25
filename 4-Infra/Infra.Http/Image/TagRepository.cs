using Common.Core.DependencyInjection;
using Core.Options;
using Domain.Image;
using Domain.Image.Repositories;
using System.Text.Json;

namespace Infra.Http.Image
{
    [ServiceLocate(typeof(ITagRepository))]
    public class TagRepository : ITagRepository
    {
        private readonly HttpClient _httpClient;
        private readonly RegistryOption _option;

        public TagRepository(IHttpClientFactory httpClientFactory, RegistryOption option)
        {
            _httpClient = httpClientFactory.CreateClient();
            _option = option;

            _httpClient.BaseAddress = new Uri(_option.BaseUrl);
        }

        public async Task<List<ITag>?> Load(IRepositoryImage image)
        {
            var relativeUrl = string.Concat("v2/", image.Name, "/tags/list");
            using var response = await _httpClient.GetAsync(new Uri(relativeUrl, UriKind.Relative)).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to load tags for image {image.Name}. Status code: {response.StatusCode}");
            }

            var contentTags = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            
            return JsonSerializer.Deserialize<List<ITag>>(contentTags);
        }
    }
}
