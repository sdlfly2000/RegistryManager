using Domain.Image;

namespace Application.Image
{
    public class ImageListWithTagsResponse : AppResponse
    {
        public ImageWithTags? Image { get; set; }
    }
}
