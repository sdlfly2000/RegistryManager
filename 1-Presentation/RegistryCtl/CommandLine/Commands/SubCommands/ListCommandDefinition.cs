using Common.Core.DependencyInjection;
using RegistryCtl.CommandLine.Commands.Actions;
using System.CommandLine;
using System.Diagnostics;

namespace RegistryCtl.CommandLine.Commands.SubCommands
{
    [ServiceLocate(default)]
    public class ListCommandDefinition
    {
        private readonly ListImageWithTagsAction _listImageWithTagsAction;
        private readonly ListAllImageAction _listAllImageAction;

        private Option<bool>? _displayAllOption;
        private Option<string>? _displayImageNameOption;

        public ListCommandDefinition(ListImageWithTagsAction listImageWithTagsAction, ListAllImageAction listAllImageAction)
        {
            _listImageWithTagsAction = listImageWithTagsAction;
            _listAllImageAction = listAllImageAction;
        }

        public Command Create()
        {
            // Define options
            _displayAllOption = new Option<bool>("--all", "-a")
            {
                DefaultValueFactory = (_) => false,
                Description = "all images."
            };

            _displayImageNameOption = new Option<string>("--name", "-n")
            {
                Description = "image name."
            };

            var listCommand = new Command("ls", "List image in the registry.")
            {
                _displayAllOption,
                _displayImageNameOption
            };

            listCommand.SetAction(SetAction);

            return listCommand;
        }

        private async Task SetAction(ParseResult parseResult, CancellationToken token)
        {
            Debug.Assert(_displayAllOption is not null);
            Debug.Assert(_displayImageNameOption is not null);

            var isAll = parseResult.GetResult(_displayAllOption)?.GetValueOrDefault<bool>() ?? false;
            var imageName = parseResult.GetResult(_displayImageNameOption)?.GetValueOrDefault<string>();

            if (isAll)
            {
                await _listAllImageAction.Act(token).ConfigureAwait(false);
            }
            else 
            {
                if (!string.IsNullOrEmpty(imageName))
                {
                    await _listImageWithTagsAction.Act(imageName, token).ConfigureAwait(false);
                }
            }
        }
    }
}
