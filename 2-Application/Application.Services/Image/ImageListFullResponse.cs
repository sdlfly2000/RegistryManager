using Domain.Image;

namespace Application.Image
{
    public class ImageListFullResponse : AppResponse
    {
        public List<RepositoryImage> Images { get; set; } = new List<RepositoryImage>();
    }
}
