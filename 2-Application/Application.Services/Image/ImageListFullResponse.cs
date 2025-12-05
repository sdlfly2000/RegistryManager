using Domain.Image;

namespace Application.Image
{
    public class ImageListFullResponse : AppResponse
    {
        public ImageListFullResponse() : base()
        {
        }

        public ImageListFullResponse(string errorMessage) : base(errorMessage)
        {
        }

        public List<RepositoryImage> Images { get; set; } = new List<RepositoryImage>();
    }
}
