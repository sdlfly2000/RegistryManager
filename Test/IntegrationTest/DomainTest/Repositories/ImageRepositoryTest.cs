using Core.Test;
using Domain.Image.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Test.Repositories;

[TestClass]
public class ImageRepositoryTest : BaseIntegrationTest
{
    [TestMethod, TestCategory(nameof(EnumTestCategory.IntegrationTest))]
    public async Task GIVEN_Image_WHEN_LoadFullList_THEN_Images_Retern()
    {
        // Arrange
        var service = _serviceProvider.GetRequiredService<IImageRepository>();

        // Action
        var images = await service.LoadFullList(CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.IsNotNull(images);
    }
}
