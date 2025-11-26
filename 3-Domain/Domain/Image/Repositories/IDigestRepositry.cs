namespace Domain.Image.Repositories
{
    public interface IDigestRepositry
    {
        Task<IDigest?> Load(IRepositoryImage image, ITag tag, CancellationToken token);
    }
}
