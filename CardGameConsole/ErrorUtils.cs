using System;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Debugging;

namespace CardGameConsole
{
    public class ErrorUtils
    {
        public static void PrintError(ScriptRuntimeException exception)
        {
            var splitted = exception.DecoratedMessage.Split(':');
            var scriptName = splitted[0].Trim();
            var msg = splitted[2].Trim();
            Console.Error.WriteLine($"[Erreur de script] : {scriptName} -> {msg}");
            for (var index = 0; index < exception.CallStack.Count; index++)
            {
                var watchItem = exception.CallStack[index];
                Console.Error.Write(
                    $"[Erreur de script] : Dans {watchItem.Name} {FormatSourceLocation(watchItem.Location)} : ");
                ColoredSource(scriptName, watchItem.Location, index == exception.CallStack.Count - 1);
                Console.WriteLine("");
            }
        }

        private static void ColoredSource(string scriptName, SourceRef watchItemLocation, bool isError)
        {
            var scriptContent = ConsoleGame.Game.GetScriptByName(scriptName);
            if (scriptContent == null) return;

            bool isMultiLine = watchItemLocation.FromLine != watchItemLocation.ToLine;

            var strings = scriptContent.Split('\n');
            string firstLine = strings[watchItemLocation.FromLine - 1];
            string lastLine = strings[watchItemLocation.ToLine - 1];
            if (!isMultiLine)
            {
                for (var i = 0; i < firstLine.Length; i++)
                {
                    if (i < watchItemLocation.FromChar || i > watchItemLocation.ToChar)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = isError ? ConsoleColor.Red : ConsoleColor.Magenta;
                    }

                    Console.Write(firstLine[i]);
                }
            }
            else
            {
                for (var i = 0; i < Math.Min(firstLine.Length, 6); i++)
                {
                    Console.Write(firstLine[i]);
                }

                Console.Write("...");
                for (var i = 0; i < Math.Min(lastLine.Length, 6); i++)
                {
                    Console.Write(lastLine[i]);
                }
            }
        }

        private static string FormatSourceLocation(SourceRef location)
        {
            return $"({location.FromLine},{location.FromChar})-({location.ToLine},{location.ToChar})";
        }
    }
}