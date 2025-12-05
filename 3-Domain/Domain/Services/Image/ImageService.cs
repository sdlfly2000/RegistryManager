using Common.Core.DependencyInjection;
using Domain.Image;
using Domain.Image.Repositories;

namespace Domain.Services.Image;

[ServiceLocate(typeof(IImageService))]
public class ImageService : IImageService
{
    private readonly ITagRepository _tagRepository;

    public ImageService(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<ImageWithTags> LoadImageWithTags(IRepositoryImage image, CancellationToken token)
    {
        var tags = await _tagRepository.Load(image, token);

        var imageWithTags = new ImageWithTags(image);

        if(tags is not null)  
            imageWithTags.Tags.AddRange(tags);

        return imageWithTags;
    }
}
