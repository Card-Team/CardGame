using System;
using System.Collections;
using System.Collections.Generic;
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

        private string finalString = "";


        public override int Execute(CommandContext context, Settings settings)
        {
            var allTypes = typeof(Game).Assembly.DefinedTypes
                .Where(IsVisibleToLua).ToList();

            var effectTypes = new List<Type>();

            foreach (var typeInfo in allTypes)
            {
                AnsiConsole.Write(new Markup($"[green] Found [bold]{typeInfo.Name}[/][/]"));
                finalString += "\n\n" + (typeInfo.IsEnum ? BuildEnumComment(typeInfo) : BuildTypeComments(typeInfo));
                if (typeof(Event).IsAssignableFrom(typeInfo)) effectTypes.Add(typeInfo);
            }

            finalString += "\n\n-- EFFETS\n\n";

            finalString += $"{Comment("class")} Type\n\n";

            foreach (var effectType in effectTypes)
                finalString += $"{GetNameWithoutGenericArity(effectType)} = --[[---@type Type]] {{}}\n\n";

            AnsiConsole.Write(new Panel(Markup.Escape(finalString)));
            File.WriteAllText(settings.outputPath, finalString);
            return 0;
        }

        private readonly string[] blacklist =
        {
            "IPropertyChange", "Exception", "IEventHandler", "Effect", "EventManager", "IExternCallbacks",
            "GetEnumerator"
        };

        private string BuildEnumComment(TypeInfo enumInfo)
        {
            var builder = new StringBuilder();

            builder.AppendLine($"{Comment("class")} {enumInfo.Name}");

            var enumNames = enumInfo.GetFields()
                .Where(f => f.IsLiteral)
                .Select(f => f.Name);

            foreach (var enumName in enumNames)
                builder.AppendLine($"{Comment("field")} public {enumName} {enumInfo.Name}");

            return builder.ToString();
        }

        private string BuildTypeComments(TypeInfo typeInfo)
        {
            //base stuff
            var builder = new StringBuilder();

            builder.AppendLine(ClassComment(typeInfo));

            //fields

            foreach (var propertyInfo in typeInfo.DeclaredProperties.Where(IsVisibleToLua))
                builder.AppendLine(PropertyComment(propertyInfo));

            //methods

            foreach (var propertyInfo in typeInfo.DeclaredMethods.Where(IsVisibleToLua))
                builder.AppendLine(MethodComment(propertyInfo));

            return builder.ToString();
        }

        private string MethodComment(MethodInfo propertyInfo)
        {
            var start = $"{Comment("field")} public {propertyInfo.Name} ";


            var parameters = propertyInfo.GetParameters()
                .Where(f => !f.IsOut)
                .Select(s => $"{s.Name}:{ToLuaType(s.ParameterType)}");

            start += $"fun({string.Join(",", parameters)})";

            if (propertyInfo.ReturnType != typeof(void)) start += $":{ToLuaType(propertyInfo.ReturnType)}";

            return start;
        }

        private string PropertyComment(PropertyInfo propertyInfo)
        {
            var start = $"{Comment("field")} public";
            var indexArray = $"{propertyInfo.Name} ";
            if (propertyInfo.GetIndexParameters().Length > 0)
            {
                var indexParams = string.Join(",",
                    propertyInfo.GetIndexParameters().Select(t => ToLuaType(t.ParameterType)));
                indexArray = $"[{indexParams}] ";
            }

            return $"{start} {indexArray}{ToLuaType(propertyInfo.PropertyType)}";
        }


        public static string GetNameWithoutGenericArity(Type t)
        {
            var name = t.Name;
            var index = name.IndexOf('`');
            return index == -1 ? name : name.Substring(0, index);
        }

        private string ToLuaType(Type propertyInfoPropertyType)
        {
            if (propertyInfoPropertyType.IsGenericType)
            {
                //todo peut etre ajouter list pour avoir contains et tout a moins que moonsharp transforme en tableau
                //voir IEnumerable 
                if (typeof(IEnumerable).IsAssignableFrom(propertyInfoPropertyType))
                    return $"{ToLuaType(propertyInfoPropertyType.GetGenericArguments()[0])}[]";

                if (propertyInfoPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    return $"{ToLuaType(propertyInfoPropertyType.GetGenericArguments()[0])} | nil";

                var genArgs = string.Join(",", propertyInfoPropertyType.GetGenericArguments().Select(ToLuaType));
                return $"{GetNameWithoutGenericArity(propertyInfoPropertyType.GetGenericTypeDefinition())}<{genArgs}>";
            }


            if (IsNumericType(propertyInfoPropertyType.GetTypeInfo())) return "number";

            if (propertyInfoPropertyType == typeof(bool)) return "boolean";

            if (propertyInfoPropertyType == typeof(string)) return "string";

            if (propertyInfoPropertyType.Name == "Closure") return "fun()";


            return propertyInfoPropertyType.Name;
        }

        private string ClassComment(TypeInfo typeInfo)
        {
            var classComment = $"{Comment("class")} {ToLuaType(typeInfo)}";
            if (typeInfo.BaseType != null && typeInfo.BaseType != typeof(object) && IsVisibleToLua(typeInfo)
                && typeInfo.BaseType.Assembly == typeInfo.Assembly)
                classComment += $" : {ToLuaType(typeInfo.BaseType)}";

            return classComment;
        }

        private string Comment(string type)
        {
            return $"---@{type}";
        }


        private bool IsBlacklisted(Type arg)
        {
            if (typeof(Delegate).IsAssignableFrom(arg)) return true;
            return IsBlacklisted(arg.Name);
        }

        private bool IsBlacklisted(string arg)
        {
            return blacklist.Any(arg.Contains);
        }

        private bool IsBlacklisted(PropertyInfo propertyInfo)
        {
            return IsBlacklisted(propertyInfo.Name) || IsBlacklisted(propertyInfo.PropertyType);
        }

        private bool IsBlacklisted(MethodInfo methodInfo)
        {
            return IsBlacklisted(methodInfo.Name) || IsBlacklisted(methodInfo.ReturnType) ||
                   methodInfo.GetParameters().Any(IsBlacklisted);
        }

        private bool IsBlacklisted(ParameterInfo arg)
        {
            return IsBlacklisted(arg.ParameterType);
        }

        private bool IsVisibleToLua(TypeInfo arg)
        {
            return !IsBlacklisted(arg) && arg.IsVisible;
        }

        private bool IsVisibleToLua(PropertyInfo arg)
        {
            if (arg.IsSpecialName) return false;
            if (IsBlacklisted(arg)) return false;
            var hasMoonSharp = arg.CustomAttributes.FirstOrDefault(f =>
                f.AttributeType.Name == "MoonSharpVisibleAttribute");
            if (hasMoonSharp != null)
                return IsVisibleToLua(arg.PropertyType.GetTypeInfo()) &&
                       (hasMoonSharp.ConstructorArguments[0].Value as bool? ?? false);
            return IsVisibleToLua(arg.PropertyType.GetTypeInfo()) && arg.GetAccessors().Any();
        }

        private bool IsVisibleToLua(MethodInfo arg)
        {
            if (arg.IsSpecialName) return false;
            if (IsBlacklisted(arg)) return false;
            var hasMoonSharp = arg.CustomAttributes.FirstOrDefault(f =>
                f.AttributeType.Name == "MoonSharpVisibleAttribute");
            if (hasMoonSharp != null)
                return IsVisibleToLua(arg.ReturnType.GetTypeInfo()) && arg.GetParameters()
                           .Select(p => p.ParameterType.GetTypeInfo())
                           .All(IsVisibleToLua) &&
                       (hasMoonSharp.ConstructorArguments[0].Value as bool? ?? false);
            return arg.IsPublic && arg.GetParameters()
                .Select(p => p.ParameterType.GetTypeInfo())
                .All(IsVisibleToLua);
        }


        public static bool IsNumericType(TypeInfo o)
        {
            switch (Type.GetTypeCode(o))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                case TypeCode.Empty:
                case TypeCode.Object:
                case TypeCode.DBNull:
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.DateTime:
                case TypeCode.String:
                default:
                    return false;
            }
        }
    }
}