using Common.Core.DependencyInjection;
using System.CommandLine;

namespace RegistryCtl.CommandLine.Commands
{
    [ServiceLocate(typeof(ICommandDefinition))]
    public class ImageCommandDefinition : ICommandDefinition
    {
        private readonly ListCommandDefinition _listCommandDefinition;

        public ImageCommandDefinition(ListCommandDefinition listCommandDefinition)
        {
            _listCommandDefinition = listCommandDefinition;
        }

        public Command Create()
        {
            var imageCommand = new Command("image", "operation on image");

            imageCommand.Subcommands.Add(_listCommandDefinition.Create());

            return imageCommand;
        }

    }
}
