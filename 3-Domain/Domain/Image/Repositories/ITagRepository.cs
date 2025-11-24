namespace Domain.Image.Repositories
{
    public interface ITagRepository
    {
        Task<List<ITag>?> Load(string imageName);
    }
}
