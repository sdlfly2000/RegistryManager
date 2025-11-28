using Application.Image;

namespace Application.Services.Image
{
    public interface IImageAppService
    {
        Task<ImageListFullResponse> List(ImageListFullRequest request, CancellationToken token);
        Task<ImageListWithTagsResponse> List(ImageListWithTagsRequest request, CancellationToken token);
    }
}
