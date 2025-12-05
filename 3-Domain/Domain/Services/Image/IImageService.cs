using Domain.Image;

namespace Domain.Services.Image;

public interface IImageService
{
    Task<ImageWithTags> LoadImageWithTags(IRepositoryImage image, CancellationToken token);
}
