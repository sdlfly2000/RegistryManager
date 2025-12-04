using System.CommandLine;

namespace RegistryCtl.CommandLine.Commands
{
    public interface ICommandDefinition
    {
        public Command Create();
    }
}
