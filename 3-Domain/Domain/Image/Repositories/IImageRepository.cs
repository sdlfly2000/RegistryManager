namespace Domain.Image.Repositories
{
    public interface IImageRepository
    {
        Task<IRepositoryImage> Load(string imageName);
    }
}
