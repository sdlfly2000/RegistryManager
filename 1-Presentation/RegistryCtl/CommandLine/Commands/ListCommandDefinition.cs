using Application.Services.Image;
using Common.Core.DependencyInjection;
using System.CommandLine;
using System.Diagnostics;
using System.Text;

namespace RegistryCtl.CommandLine.Commands
{
    [ServiceLocate(default)]
    public class ListCommandDefinition
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
                var response = await _imageAppService.List(new Application.Image.ImageListFullRequest(), token).ConfigureAwait(false);
                if (response.Success)
                {
                    var output = new StringBuilder();
                    output.AppendLine();
                    output.AppendLine("IMAGE LIST:");
                    
                    response.Images.ForEach(image =>
                    {
                        output.AppendLine($"{image.Name}");
                    });
                }
            }
        }
    }
}
