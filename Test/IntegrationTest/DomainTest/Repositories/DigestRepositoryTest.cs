using Core.Test;
using Domain.Image;
using Domain.Image.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Test.Repositories;

[TestClass]
public class DigestRepositryTest : BaseIntegrationTest
{
    [TestMethod, TestCategory(nameof(EnumTestCategory.IntegrationTest))]
    public async Task GIVEN_Tag_WHEN_Load_THEN_Digestd_Retern()
    {
        // Arrange
        var image = new RepositoryImage("authservice/authservice");
        var tag = new Tag("20251103-151324");
        var service = _serviceProvider.GetRequiredService<IDigestRepositry>();

        // Action
        var digest = await service.Load(image, tag, CancellationToken.None).ConfigureAwait(false);
        var digestFromMemory = await service.Load(image, tag, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.IsNotNull(digest);
        Assert.IsNotNull(digestFromMemory);
        Assert.AreEqual(digest.Code, digestFromMemory.Code);
    }
}
