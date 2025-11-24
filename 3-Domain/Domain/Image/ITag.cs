namespace Domain.Image
{
    public interface ITag
    {
        public string Name { get; set; }
        public IDigest Digest { get; set; }
    }
}
