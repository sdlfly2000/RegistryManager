using Domain.Image;

namespace Application.Image
{
    public class ImageListWithTagsRequest : AppRequest
    {
        public required RepositoryImage Image { get; set; }
    }
}
