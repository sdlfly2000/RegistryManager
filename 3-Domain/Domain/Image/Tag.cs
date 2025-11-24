namespace Domain.Image
{
    public class Tag : ITag
    {
        public required string Name { get; set; }
        public required IDigest Digest { get; set; }
    }
}
