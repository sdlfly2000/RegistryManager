using System.Text;

namespace Core.CommandLine
{
    public static class CommandLineFormatter
    {
        public static void Format(string title, List<string> contents)
        {
            var output = new StringBuilder();
            output.AppendLine();
            output.AppendLine(title);
            output.AppendLine(new string('=', title.Length * 2));

            contents.ForEach(content =>
            {
                output.AppendLine($"{content}");
            });

            Console.WriteLine(output.ToString());
        }
    }
}
