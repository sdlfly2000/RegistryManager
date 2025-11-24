namespace Domain.Image
{
    public class RepositoryImage : IRepositoryImage
    {
        public RepositoryImage(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
