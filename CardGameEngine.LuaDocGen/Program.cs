using Spectre.Console.Cli;

namespace CardGameEngine.LuaDocGen
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var app = new CommandApp<GenerateCommand>();
            app.Run(args);
        }
    }
}