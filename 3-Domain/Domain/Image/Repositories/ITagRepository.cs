namespace Domain.Image.Repositories
{
    public interface ITagRepository
    {
        Task<List<ITag>?> Load(IRepositoryImage image, CancellationToken token);
    }
}
