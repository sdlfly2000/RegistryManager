using Core.Test;
using Domain.Image;
using Domain.Image.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Test.Repositories;

[TestClass]
public class ImageRepositoryTest : BaseIntegrationTest
{
    [TestMethod, TestCategory(nameof(EnumTestCategory.IntegrationTest))]
    public async Task GIVEN_Image_WHEN_Load_THEN_Tags_Retern()
    {
        // Arrange
        var image = new RepositoryImage("authservice/authservice");
        var service = _serviceProvider.GetRequiredService<ITagRepository>();

        // Action
        var tags = await service.Load(image, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.IsNotNull(tags);
    }
}
