namespace Application.Image
{
    public class ImageTagDeleteRequest : AppRequest
    {
        public required string ImageName { get; set; }
        public required string TagName { get; set; }
    }
}
