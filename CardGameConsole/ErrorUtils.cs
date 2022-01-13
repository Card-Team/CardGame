using System;
using System.Collections.Generic;
using System.Linq;
using CardGameEngine;
using CardGameEngine.GameSystems.Effects;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Debugging;
using Spectre.Console;

namespace CardGameConsole
{
    public static class ErrorUtils
    {
        private const string ScriptError = "[red][[Erreur de script]][/] : ";

        public static void PrintError(InterpreterException exc)
        {
            PrintError(exc, exc.CallStack.ToList());
        }

        public static void PrintError(InterpreterException exception, List<WatchItem> callstack)
        {
            var splitted = exception.DecoratedMessage.Split(':').ToList();
            var scriptName = string.Join(":", splitted.GetRange(0, splitted.Count - 2));
            var msg = exception.Message;
            AnsiConsole.Write(new Markup($"[[Erreur de script]] : [blue]{scriptName}[/] -> [underline]{msg}[/]\n"));
            for (var index = 0; index < callstack.Count; index++)
            {
                var watchItem = callstack[index];
                var text =
                    $"[[Erreur de script]] : Dans [blue]{watchItem.Name}[/] [underline]{FormatSourceLocation(watchItem.Location)}[/]";
                if (watchItem.Location is { IsClrLocation: false } && (!watchItem.Name?.StartsWith("<") ?? true))
                {
                    text += " : " + ColoredSource(scriptName, text.Length - ScriptError.Length - 2, watchItem.Location,
                        index == 0) + "\n";
                }

                AnsiConsole.Write(new Markup(text));
            }

            DumpEvents();
        }

        private static void DumpEvents()
        {
            var dumpEvents = EventDisplayer.DumpEvents();

            if (dumpEvents != null)
                AnsiConsole.Write(dumpEvents.Header("Derniers évenements"));
        }

        public static void PrintError(InvalidOperationException exception)
        {
            AnsiConsole.Write(new Markup($"[red][[Erreur Moteur]] : {exception.Message}[/]"));
            AnsiConsole.WriteException(exception);
            DumpEvents();
        }

        private static string ColoredSource(string scriptName, int padding, SourceRef watchItemLocation, bool isError)
        {
            var scriptContent = ConsoleGame.Game.GetScriptByName(scriptName);
            if (scriptContent == null) return "";

            bool isMultiLine = watchItemLocation.FromLine != watchItemLocation.ToLine && watchItemLocation.ToChar != 0;

            var strings = scriptContent.Split('\n');
            string firstLine = strings[watchItemLocation.FromLine - 1];
            string lastLine = strings[watchItemLocation.ToLine - 1];

            var accumulator = "";

            if (!isMultiLine)
            {
                var before = Console.ForegroundColor;
                for (var i = 0; i < firstLine.Length; i++)
                {
                    if (i == watchItemLocation.FromChar)
                    {
                        accumulator += "[red]";
                    }
                    else if (watchItemLocation.ToChar == 0 && i == firstLine.Length - 1 ||
                             watchItemLocation.ToChar != 0 && i == watchItemLocation.ToChar)
                    {
                        accumulator += "[/]";
                    }

                    accumulator += firstLine[i];
                }

                return accumulator;
            }
            else
            {
                accumulator += "|";
                foreach (var t in firstLine)
                {
                    accumulator += t;
                }

                accumulator += "\n";

                accumulator += ScriptError;
                for (var i = 0; i < padding; i++)
                {
                    accumulator += " ";
                }

                accumulator += "\n|";
                accumulator += "...\n";

                accumulator += ScriptError;
                for (var i = 0; i < padding; i++)
                {
                    accumulator += " ";
                }

                accumulator += "\n|";
                foreach (var t in lastLine)
                {
                    accumulator += t;
                }

                return accumulator;
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

        public static void PrintError(InvalidEffectException exception)
        {
            AnsiConsole.Write(new Markup($"[red][[Effet invalide]]: {exception.Message}[/]"));
            if (exception.InnerException != null)
                PrintError(exception.InnerException);
            else
                DumpEvents();
        }
    }
}