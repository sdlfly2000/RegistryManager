using Domain.Image;

namespace Domain.Services.Image;

public interface IImageService
{
    Task<IImageWithTags> LoadImageWithTags(IRepositoryImage image, CancellationToken token);
}
