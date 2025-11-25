namespace Domain.Image.Repositories
{
    public interface IDigestRepositry
    {
        Task<IDigest?> Load(IRepositoryImage image, string tagName, CancellationToken token);
    }
}
