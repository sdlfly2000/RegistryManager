using Common.Core.DependencyInjection;
using RegistryCtl.CommandLine.Commands;
using System.CommandLine;

namespace RegistryCtl.CommandLine
{
    [ServiceLocate(default)]
    public class CommandLineParser
    {
        private readonly ListCommandDefinition _listCommandDefinition;

        public CommandLineParser(ListCommandDefinition listCommandDefinition)
        {
            _listCommandDefinition = listCommandDefinition;
        }

        public ParseResult Parse(string[] args)
        {
            // Define commands
            var rootCommand = new RootCommand("Manage Private Registry");

            rootCommand.Subcommands.Add(_listCommandDefinition.Create());

            var parseResult = rootCommand.Parse(args);

            return parseResult;
        }
    }
}
