using Common.Core.DependencyInjection;
using RegistryCtl.CommandLine.Commands.Actions;
using System.CommandLine;
using System.Diagnostics;

namespace RegistryCtl.CommandLine.Commands.SubCommands
{
    [ServiceLocate(default)]
    public class DeleteCommandDefinition
    {
        private readonly DeleteImageTagAction _deleteImageTagAction;

        private Option<string>? _displayImageNameOption;
        private Option<string>? _displayTagNameOption;

        public DeleteCommandDefinition(DeleteImageTagAction deleteImageTagAction)
        {
            _deleteImageTagAction = deleteImageTagAction;
        }

        public Command Create()
        {
            _displayImageNameOption = new Option<string>("--name", "-n")
            {
                Required = true,
                Description = "image name."
            };

            _displayTagNameOption = new Option<string>("--tag", "-t")
            {
                Required = true,
                Description = "tag name."
            };

            var deleteCommand = new Command("rm", "List image in the registry.")
            {
                _displayTagNameOption,
                _displayImageNameOption
            };

            deleteCommand.SetAction(SetAction);

            return deleteCommand;
        }

        private async Task SetAction(ParseResult parseResult, CancellationToken token)
        {
            Debug.Assert(_displayTagNameOption is not null);
            Debug.Assert(_displayImageNameOption is not null);

            var tagName = parseResult.GetResult(_displayTagNameOption)?.GetValueOrDefault<string>() ?? string.Empty;
            var imageName = parseResult.GetResult(_displayImageNameOption)?.GetValueOrDefault<string>() ?? string.Empty;

            await _deleteImageTagAction.Act(imageName, tagName, token);

        }
    }
}
