using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CardGameEngine.EventSystem.Events;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CardGameEngine.LuaDocGen
{
    public class GenerateCommand : Command<GenerateCommand.Settings>
    {
        public sealed class Settings : CommandSettings
        {
            [Description("Chemin du fichier Lua de sortie")]
            [CommandArgument(0, "[sortie]")]
            public string outputPath { get; set; }
        }


        public override int Execute(CommandContext context, Settings settings)
        {
            var allTypes = typeof(Game).Assembly.DefinedTypes
                .Where(Visibility.IsVisibleToLua).Concat(new List<TypeInfo>
                {
                    typeof(List<>).GetTypeInfo(),
                    typeof(ReadOnlyCollection<>).GetTypeInfo(),
                    typeof(IEnumerable<>).GetTypeInfo(),
                    typeof(IComparer<>).GetTypeInfo(),
                    typeof(object).GetTypeInfo()
                }).ToList();

            var effectTypes = new List<Type>();

            var finalBuilder = new StringBuilder();
            foreach (var typeInfo in allTypes)
            {
                AnsiConsole.Write(new Markup($"[green] Found [bold]{typeInfo.Name}[/][/]"));
                finalBuilder.AppendLine(typeInfo.IsEnum
                    ? CommentBuilding.BuildEnumComment(typeInfo)
                    : CommentBuilding.BuildTypeComments(typeInfo));
                finalBuilder.AppendLine();
                if (typeof(Event).IsAssignableFrom(typeInfo)) effectTypes.Add(typeInfo);
            }

            finalBuilder.AppendLine();
            finalBuilder.AppendLine();

            finalBuilder.AppendLine(BuildEffectTypes(effectTypes));

            var res = finalBuilder.ToString();
            AnsiConsole.Write(new Panel(Markup.Escape(res)));
            File.WriteAllText(settings.outputPath, res);
            return 0;
        }

        private static string BuildEffectTypes(List<Type> effectTypes)
        {
            var builder = new StringBuilder();
            builder.AppendLine("-- EFFETS");
            builder.AppendLine();
            builder.AppendLine();

            builder.AppendLine($"{CommentBuilding.Comment("class")} Type<T:Event>");
            builder.AppendLine();

            foreach (var effectType in effectTypes)
            {
                var typename = effectType.GetNameWithoutGenericArity();
                var genericname = typename;
                if (typename == "CardPropertyChangeEvent") genericname = "CardPropertyChangeEvent<any>";
                builder.AppendLine($"---@type Type<{genericname}>");
                builder.AppendLine($"T_{typename} = --[[---@type Type<{genericname}>]] {{}}");
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}