namespace Domain.Image
{
    public class ImageWithTags : IImageWithTags
    {
        private IRepositoryImage _image;

        public List<ITag> Tags { get; }

        public string Name { get => _image.Name; }

        public ImageWithTags(IRepositoryImage image)
        {
            _image = image;
            Tags = new List<ITag>();
        }
    }
}
