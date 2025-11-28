namespace Domain.Image.Repositories
{
    public interface IImageRepository
    {
        Task<IList<RepositoryImage>> LoadFullList(CancellationToken token);
    }
}
