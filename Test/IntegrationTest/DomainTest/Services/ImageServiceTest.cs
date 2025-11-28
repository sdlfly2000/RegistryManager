using Core.Test;
using Domain.Image;
using Domain.Services.Image;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Test.Services;

[TestClass]
public class ImageServiceTest : BaseIntegrationTest
{
    [TestMethod, TestCategory(nameof(EnumTestCategory.IntegrationTest))]
    public async Task GIVEN_Image_WHEN_LoadImageWithTags_THEN_ImageWithTags_Retern()
    {
        // Arrange
        var image = new RepositoryImage("authservice/authservice");
        var service = _serviceProvider.GetRequiredService<IImageService>();

        // Action
        var imageWithTags = await service.LoadImageWithTags(image, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.IsNotNull(imageWithTags);
    }
}
