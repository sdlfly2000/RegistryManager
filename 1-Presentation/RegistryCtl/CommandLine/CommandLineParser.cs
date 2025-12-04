using Common.Core.DependencyInjection;
using RegistryCtl.CommandLine.Commands;
using System.CommandLine;

namespace RegistryCtl.CommandLine
{
    [ServiceLocate(default)]
    public class CommandLineParser
    {
        private readonly IEnumerable<ICommandDefinition> _commandDefinitions;

        public CommandLineParser(IEnumerable<ICommandDefinition> commandDefinitions)
        {
            _commandDefinitions = commandDefinitions;
        }

        public ParseResult Parse(string[] args)
        {
            // Define commands
            var rootCommand = new RootCommand("Manage Private Registry");

            foreach (var commandDefinition in _commandDefinitions)
            {
                rootCommand.Subcommands.Add(commandDefinition.Create());
            }

            var parseResult = rootCommand.Parse(args);

            return parseResult;
        }
    }
}
