using Application.Image;
using Common.Core.DependencyInjection;
using Domain.Image.Repositories;
using Domain.Services.Image;

namespace Application.Services.Image
{
    [ServiceLocate(typeof(IImageAppService), ServiceType.Scoped)]
    public class ImageAppService : IImageAppService
    {
        private readonly IImageService _imageService;
        private readonly IImageRepository _imageRepository;

        public ImageAppService(IImageService imageService, IImageRepository imageRepository)
        {
            _imageService = imageService;
            _imageRepository = imageRepository;
        }

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

        public async Task<ImageListWithTagsResponse> List(ImageListWithTagsRequest request, CancellationToken token)
        {
            var imageWithTags = await _imageService.LoadImageWithTags(request.Image, token).ConfigureAwait(false);

            return new ImageListWithTagsResponse
            {
                Success = true,
                Image = imageWithTags
            };
        }
    }
}
