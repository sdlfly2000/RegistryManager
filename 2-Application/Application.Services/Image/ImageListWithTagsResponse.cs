using Domain.Image;

namespace Application.Image
{
    public class ImageListWithTagsResponse : AppResponse
    {
        public ImageListWithTagsResponse() : base()
        {
        }

        public ImageListWithTagsResponse(string errorMessage) : base(errorMessage)
        {
            
        }

        public ImageWithTags? Image { get; set; }
    }
}
