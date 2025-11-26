using Core.AOP.Cache;

namespace Domain.Image
{
    public class RepositoryImage : IRepositoryImage
    {
        public RepositoryImage(string name)
        {
            Name = name;
        }

        [CacheKey]
        public string Name { get; }
    }
}
