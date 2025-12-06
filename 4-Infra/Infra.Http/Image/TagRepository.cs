using Common.Core.DependencyInjection;
using Core.Options;
using Domain.Image;
using Domain.Image.Repositories;
using Infra.Http.Image.model;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infra.Http.Image
{
    [ServiceLocate(typeof(ITagRepository))]
    public class TagRepository : ITagRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly RegistryOption _option;
        private readonly IDigestRepositry _digestRepository;

        public TagRepository(IHttpClientFactory httpClientFactory, IDigestRepositry digestRepository, IOptions<RegistryOption> option)
        {
            _httpClientFactory = httpClientFactory;
            _option = option.Value;
            _digestRepository = digestRepository;
        }

        public async Task Delete(IRepositoryImage image, string tagName, CancellationToken token)
        {
            var tags = await Load(image, token).ConfigureAwait(false);
            var tag = tags?.Single(t => t.Name == tagName);

            using var httpClient = CreateHttpClient();
            var relativeUrl = string.Concat("v2/", image.Name, "/manifests/", tag?.Digest?.Code);
            using var response = await httpClient.DeleteAsync(new Uri(relativeUrl, UriKind.Relative), token).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to delete tags for image {image.Name}. Status code: {response.StatusCode}");
            }
        }

        public async Task<List<ITag>?> Load(IRepositoryImage image, CancellationToken token)
        {
            var tags = new List<Tag>();

            using var httpClient = CreateHttpClient();

            var relativeUrl = string.Concat("v2/", image.Name, "/tags/list");
            using var response = await httpClient.GetAsync(new Uri(relativeUrl, UriKind.Relative), token).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to load tags for image {image.Name}. Status code: {response.StatusCode}");
            }
            var contentTags = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var tagModel = JsonSerializer.Deserialize<TagModel>(contentTags);

            if (tagModel != null)
            {
                foreach (var tagName in tagModel.tags)
                {
                    var tag = new Tag(tagName);
                    tag.Digest = await _digestRepository.Load(image, tag, token).ConfigureAwait(false);
                    tags.Add(tag);
                }
            }
            return tags.ToList<ITag>();
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
