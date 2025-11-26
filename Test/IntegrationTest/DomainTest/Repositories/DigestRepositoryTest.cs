using Core.Test;
using Domain.Image;
using Domain.Image.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Test.Repositories;

[TestClass]
public class DigestRepositoryTest : BaseIntegrationTest
{
    [TestMethod, TestCategory(nameof(EnumTestCategory.IntegrationTest))]
    public async Task GIVEN_Tag_WHEN_Load_THEN_Digestd_Retern()
    {
        // Arrange
        var image = new RepositoryImage("authservice/authservice");
        var tag = new Tag("tagName");
        var service = _serviceProvider.GetRequiredService<IDigestRepositry>();

        // Action
        var tags = await service.Load(image, tag, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.IsNotNull(tags);
    }
}
