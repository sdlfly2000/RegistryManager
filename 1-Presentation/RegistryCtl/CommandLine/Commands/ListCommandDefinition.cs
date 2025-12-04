using Application.Image;
using Application.Services.Image;
using Common.Core.DependencyInjection;
using Core.CommandLine;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics;
using System.Text;

namespace RegistryCtl.CommandLine.Commands
{
    [ServiceLocate(typeof(ICommandDefinition))]
    public class ListCommandDefinition : ICommandDefinition
    {
        private readonly IImageAppService _imageAppService;

        private Option<bool>? _displayAllOption;
        private Option<string>? _displayImageOption;

        public ListCommandDefinition(IImageAppService imageAppService)
        {
            _imageAppService = imageAppService;
        }

        public Command Create()
        {
            // Define options
            _displayAllOption = new Option<bool>("--all")
            {
                DefaultValueFactory = (_) => false,
                Description = "all images."
            };

            _displayImageOption = new Option<string>("--image")
            {
                Description = "image name."
            };

            var listCommand = new Command("list", "List image in the registry.")
            {
                _displayAllOption,
                _displayImageOption
            };

            listCommand.SetAction(SetAction);

            return listCommand;
        }

        private async Task SetAction(ParseResult parseResult, CancellationToken token)
        {
            Debug.Assert(_displayAllOption is not null);
            Debug.Assert(_displayImageOption is not null);

            var isAll = parseResult.GetResult(_displayAllOption)?.GetValueOrDefault<bool>() ?? false;
            var imageName = parseResult.GetResult(_displayImageOption)?.GetValueOrDefault<string>();

            if (isAll)
            {
                await ListAllImage(token).ConfigureAwait(false);
            }
            else 
            {
                if (!string.IsNullOrEmpty(imageName))
                {
                    await ListImageWithTags(imageName, token).ConfigureAwait(false);
                }
            }
        }

        private async Task ListImageWithTags(string imageName, CancellationToken token)
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

        private async Task ListAllImage(CancellationToken token)
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
