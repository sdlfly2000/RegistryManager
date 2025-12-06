using Application.Image;
using Common.Core.DependencyInjection;
using Core.AOP.CatchException;
using Domain.Image;
using Domain.Image.Repositories;
using Domain.Services.Image;

namespace Application.Services.Image
{
    [ServiceLocate(typeof(IImageAppService), ServiceType.Scoped)]
    public class ImageAppService : IImageAppService
    {
        private readonly IImageService _imageService;
        private readonly IImageRepository _imageRepository;
        private readonly ITagRepository _tagRepository;

        public ImageAppService(IImageService imageService, IImageRepository imageRepository, ITagRepository tagRepository)
        {
            _imageService = imageService;
            _imageRepository = imageRepository;
            _tagRepository = tagRepository;
        }

        [CatchAppException(returnType: typeof(ImageTagDeleteResponse))]
        public async Task<ImageTagDeleteResponse> Delete(ImageTagDeleteRequest request, CancellationToken token)
        {
            var image = new RepositoryImage(request.ImageName);

            await _tagRepository.Delete(image, request.TagName, token).ConfigureAwait(false);
            
            return new ImageTagDeleteResponse
            {
                Success = true
            };
        }

        [CatchAppException(returnType: typeof(ImageListFullResponse))]
        public async Task<ImageListFullResponse> List(ImageListFullRequest request, CancellationToken token)
        {
            var response = new ImageListFullResponse
            {
                Success = true
            };

            var images = await _imageRepository.LoadFullList(token).ConfigureAwait(false);

            response.Images.AddRange(images);

            return response;
        }

        [CatchAppException(returnType: typeof(ImageListWithTagsResponse))]
        public async Task<ImageListWithTagsResponse> List(ImageListWithTagsRequest request, CancellationToken token)
        {
            var image = new RepositoryImage(request.ImageName);
            var imageWithTags = await _imageService.LoadImageWithTags(image, token).ConfigureAwait(false);

            return new ImageListWithTagsResponse
            {
                Success = true,
                Image = imageWithTags
            };
        }
    }
}
