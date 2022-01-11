using System;
using System.Collections.Generic;
using System.Linq;
using CardGameEngine;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Debugging;

namespace CardGameConsole
{
    public static class ErrorUtils
    {
        private const string ScriptError = "[Erreur de script] : ";

        public static void PrintError(ScriptRuntimeException exc)
        {
            PrintError(exc, exc.CallStack.ToList());
        }

        public static void PrintError(ScriptRuntimeException exception, List<WatchItem> callstack)
        {
            var splitted = exception.DecoratedMessage.Split(':');
            var scriptName = splitted[0].Trim();
            var msg = splitted[2].Trim();
            Console.Error.WriteLine($"[Erreur de script] : {scriptName} -> {msg}");
            for (var index = 0; index < callstack.Count; index++)
            {
                var watchItem = callstack[index];
                var text = $"[Erreur de script] : Dans {watchItem.Name} {FormatSourceLocation(watchItem.Location)}";
                Console.Error.Write(
                    text);
                if (watchItem.Location is { IsClrLocation: false } && !watchItem.Name.StartsWith("<"))
                {
                    Console.Write(" : ");
                    ColoredSource(scriptName, text.Length - ScriptError.Length, watchItem.Location,
                        index == 0);
                }

                Console.WriteLine("");
            }
        }

        public static void PrintError(InvalidOperationException exception)
        {
            var bef = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Erreur Moteur] : {exception.Message}");
            Console.ForegroundColor = bef;
        }

        private static void ColoredSource(string scriptName, int padding, SourceRef watchItemLocation, bool isError)
        {
            var scriptContent = ConsoleGame.Game.GetScriptByName(scriptName);
            if (scriptContent == null) return;

            bool isMultiLine = watchItemLocation.FromLine != watchItemLocation.ToLine && watchItemLocation.ToChar != 0;

            var strings = scriptContent.Split('\n');
            string firstLine = strings[watchItemLocation.FromLine - 1];
            string lastLine = strings[watchItemLocation.ToLine - 1];
            if (!isMultiLine)
            {
                var before = Console.ForegroundColor;
                for (var i = 0; i < firstLine.Length; i++)
                {
                    if (i < watchItemLocation.FromChar ||
                        (watchItemLocation.ToChar != 0 && i > watchItemLocation.ToChar))
                    {
                        Console.ForegroundColor = before;
                    }
                    else
                    {
                        Console.ForegroundColor = isError ? ConsoleColor.Red : ConsoleColor.Magenta;
                    }

                    Console.Write(firstLine[i]);
                }

                Console.ForegroundColor = before;
            }
            else
            {
                Console.Write("|");
                foreach (var t in firstLine)
                {
                    Console.Write(t);
                }

                Console.WriteLine("");

                Console.Write(ScriptError);
                for (var i = 0; i < padding; i++)
                {
                    Console.Write(" ");
                }

                Console.Write("|");
                Console.WriteLine("...");

                Console.Write(ScriptError);
                for (var i = 0; i < padding; i++)
                {
                    Console.Write(" ");
                }

                Console.Write("|");
                foreach (var t in lastLine)
                {
                    Console.Write(t);
                }
            }
        }

        private static string FormatSourceLocation(SourceRef location)
        {
            if (location == null)
            {
                return "";
            }
            return $"({location.FromLine},{location.FromChar})-({location.ToLine},{location.ToChar})";
        }

        public static void PrintError(LuaException exception)
        {
            PrintError(exception.RuntimeException, exception.CallStack);
        }
    }
}