using Core.AOP.Cache;

namespace Domain.Image;

public class Tag : ITag
{
    public Tag(string tagName)
    {
        Name = tagName;
    }

    [CacheKey]
    public string Name { get; set; }

    public IDigest Digest { get; set; }
}
