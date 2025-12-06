using Common.Core.DependencyInjection;
using RegistryCtl.CommandLine.Commands.SubCommands;
using System.CommandLine;

namespace RegistryCtl.CommandLine.Commands
{
    [ServiceLocate(typeof(ICommandDefinition))]
    public class ImageCommandDefinition : ICommandDefinition
    {
        private readonly ListCommandDefinition _listCommandDefinition;
        private readonly DeleteCommandDefinition _deleteCommandDefinition;

        public ImageCommandDefinition(ListCommandDefinition listCommandDefinition, DeleteCommandDefinition deleteCommandDefinition)
        {
            _listCommandDefinition = listCommandDefinition;
            _deleteCommandDefinition = deleteCommandDefinition;
        }

        public Command Create()
        {
            var imageCommand = new Command("image", "operation on image");

            imageCommand.Subcommands.Add(_listCommandDefinition.Create());
            imageCommand.Subcommands.Add(_deleteCommandDefinition.Create());

            return imageCommand;
        }

    }
}
