using Application.Image;
using Application.Services.Image;
using Core.Options;
using Domain.Image;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Linq;

namespace Application.BDD.StepDefinitions
{
    [Binding]
    public class LoadImageFromPrivateDockerRegistryStepDefinitions
    {
        private readonly IOptions<RegistryOption> _registryOption;
        private readonly IImageAppService _imageAppService;

        private ImageListFullRequest? _imageListFullRequest;
        private ImageListFullResponse? _imageListFullResponse;

        public LoadImageFromPrivateDockerRegistryStepDefinitions(IImageAppService imageAppService, IOptions<RegistryOption> registryOption)
        {
            _registryOption = registryOption;
            _imageAppService = imageAppService;
        }

        [Given("Private Docker Registry Host: {string}")]
        public void GivenPrivateDockerRegistryHost(string registryHost)
        {
            Assert.AreEqual(registryHost, _registryOption.Value.BaseUrl);
        }

        [Given("Create a ImageListFullRequest")]
        public void GivenCreateAImageListWithTagsRequest()
        {
            _imageListFullRequest = new ImageListFullRequest();
        }

        [When("do List")]
        public async Task WhenDoList()
        {
            Debug.Assert(_imageListFullRequest != null, "_imageListFullRequest == null");
            _imageListFullResponse = await _imageAppService.List(_imageListFullRequest, CancellationToken.None).ConfigureAwait(false);
        }

        [Then("the result below should be returned at least:")]
        public void ThenTheResultBelowShouldBeReturnedAtLeast(DataTable dataTable)
        {
            var images = dataTable.CreateSet<RepositoryImage>((row) => new RepositoryImage(row["Name"]))
                                    .ToList();

            Assert.IsNotNull(_imageListFullResponse);

            foreach (var image in images)
            {
                Assert.IsTrue(_imageListFullResponse.Images.Any(i => i.Name.Equals(image.Name)));
            }
        }
    }
}
