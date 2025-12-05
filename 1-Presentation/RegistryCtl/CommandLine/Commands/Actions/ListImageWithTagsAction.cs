using Application.Image;
using Application.Services.Image;
using Common.Core.DependencyInjection;
using Core.CommandLine;

namespace RegistryCtl.CommandLine.Commands.Actions
{
    [ServiceLocate(default)]
    public class ListImageWithTagsAction
    {
        private readonly IImageAppService _imageAppService;

        public ListImageWithTagsAction(IImageAppService imageAppService)
        {
            _imageAppService = imageAppService;
        }

        public async Task Act(string imageName, CancellationToken token)
        {
            var response = await _imageAppService.List(new ImageListWithTagsRequest { Image = new Domain.Image.RepositoryImage(imageName) }, token);
            if (response.Success)
            {
                CommandLineFormatter.Format(imageName, response.Image!.Tags.Select(tag => tag.Name).ToList());
            }
            else
            {
                Console.WriteLine($"Error: {response.ErrorMessage}");
            }
        }
    }
}
