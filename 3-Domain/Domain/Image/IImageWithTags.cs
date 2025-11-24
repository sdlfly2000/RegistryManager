namespace Domain.Image
{
    public interface IImageWithTags : IRepositoryImage
    {
        public List<ITag> Tags { get; }
    }
}
