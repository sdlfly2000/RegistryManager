using RegistryCtl.CommandLineParser.Model;
using System.CommandLine;

namespace RegistryCtl.CommandLineParser
{
    public static class CommandLineParser
    {
        public CommandLineConfiguration Parse(string args)
        {
            Option<FileInfo> fileOption = new("--file")
            {
                Description = "The file to read and display on the console."
            };

            RootCommand rootCommand = new("Sample app for System.CommandLine");
            rootCommand.Options.Add(fileOption);

            ParseResult parseResult = rootCommand.Parse(args);

            return CommandLineConfiguration
        }
    }
}
