using Core.Test;
using Domain.Image;
using Domain.Image.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Test.Repositories;

[TestClass]
public sealed class TagRepositoryTest : BaseIntegrationTest
{
    [TestMethod, TestCategory(nameof(EnumTestCategory.IntegrationTest))]
    public void GIVEN_Image_WHEN_Load_THEN_Tags_Retern()
    {
        // Arrange
        var image = new RepositoryImage("authservice/authservice");
        var service = _serviceProvider.GetRequiredService<ITagRepository>();

        // Action
        var tags = service.Load(image);

        // Assert
        Assert.IsNotNull(tags);
    }
}
