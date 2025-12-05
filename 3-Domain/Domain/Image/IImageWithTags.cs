namespace Domain.Image
{
    public interface IImageWithTags : IRepositoryImage
    {
        public string Name { get;  }
        public List<ITag> Tags { get; }
    }
}
