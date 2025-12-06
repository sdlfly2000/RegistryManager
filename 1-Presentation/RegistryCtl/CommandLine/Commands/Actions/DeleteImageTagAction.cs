using Application.Image;
using Application.Services.Image;
using Common.Core.DependencyInjection;

namespace RegistryCtl.CommandLine.Commands.Actions
{
    [ServiceLocate(default)]
    public class DeleteImageTagAction
    {
        private readonly IImageAppService _imageAppService;

        public DeleteImageTagAction(IImageAppService imageAppService)
        {
            _imageAppService = imageAppService;
        }

        public async Task Act(string imageName,string tagName, CancellationToken token)
        {
            var response = await _imageAppService
                .Delete(new ImageTagDeleteRequest { ImageName = imageName, TagName = tagName }, token)
                .ConfigureAwait(false);

            if (response.Success)
            {
                Console.WriteLine($"Delete Tag [{tagName}] from image [{imageName}]");
            }
            else
            {
                Console.WriteLine($"Error: {response.ErrorMessage}");
            }
        }
    }
}
