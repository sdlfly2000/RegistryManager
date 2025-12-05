using Application.Services.Image;
using Common.Core.DependencyInjection;
using Core.CommandLine;

namespace RegistryCtl.CommandLine.Commands.Actions
{
    [ServiceLocate(default)]
    public class ListAllImageAction
    {
        private readonly IImageAppService _imageAppService;

        public ListAllImageAction(IImageAppService imageAppService)
        {
            _imageAppService = imageAppService;
        }

        public async Task Act(CancellationToken token)
        {
            var response = await _imageAppService.List(new Application.Image.ImageListFullRequest(), token).ConfigureAwait(false);
            if (response.Success)
            {
                CommandLineFormatter.Format("Image List", response.Images.Select(image => image.Name).ToList());
            }
            else
            {
                Console.WriteLine($"Error: {response.ErrorMessage}");
            }
        }
    }
}
