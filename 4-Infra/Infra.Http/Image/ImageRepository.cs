using Domain.Image;
using Domain.Image.Repositories;
using System.Net.Http;

namespace Infra.Http.Image
{
    public class ImageRepository : IImageRepository
    {
        public ImageRepository() 
        {

        }

        public async Task<IRepositoryImage> Load(string imageName)
        {
            throw new NotImplementedException();
        }
    }
}
