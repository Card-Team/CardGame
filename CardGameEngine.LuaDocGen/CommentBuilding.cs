using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CardGameEngine.LuaDocGen
{
    internal static class CommentBuilding
    {
        public static string BuildEnumComment(Type enumInfo)
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

        public static string BuildTypeComments(TypeInfo typeInfo)
        {
            //base stuff
            var builder = new StringBuilder();

            builder.AppendLine(ClassComment(typeInfo));

            //fields

            foreach (var propertyInfo in typeInfo.DeclaredProperties.Where(Visibility.IsVisibleToLua))
                builder.AppendLine(PropertyComment(propertyInfo));

            // stub

            var varname = typeInfo.GetNameWithoutGenericArity();

            builder.AppendLine($"{varname} = {{}}");


            //methods

            foreach (var propertyInfo in typeInfo.DeclaredMethods.Where(Visibility.IsVisibleToLua))
                builder.AppendLine(MethodComment(varname, propertyInfo));

            return builder.ToString();
        }

        private static string MethodComment(string typename, MethodInfo methodInfo)
        {
            var builder = new StringBuilder();
            builder.AppendLine("--- Documentation a venir");
            var whered = methodInfo.GetParameters()
                .Where(f => !f.IsOut);
            var parameters = whered
                .Select(MapParameter)
                .Select(s => $"{Comment("param")} {s.Name} {s.Type} Documentation a venir");

            if (methodInfo.GetParameters().Length > 0) builder.AppendLine(string.Join("\n", parameters));

            var methodName = methodInfo.Name;
            if (methodName == "GetEnumerator")
            {
                methodName = "__iterator";
                builder.AppendLine(
                    $"{Comment("return")} fun():T Documentation a venir");
            }
            else
            {
                if (methodInfo.ReturnType != typeof(void))
                    builder.AppendLine(
                        $"{Comment("return")} {methodInfo.ReturnType.ToLuaType()} Documentation a venir");
            }

            if (methodInfo.GetParameters().Any(p => p.IsOptional))
            {
                //overload without optional
                var parlist = methodInfo.GetParameters().Where(p => !p.IsOptional)
                    .Select(s => $"{s.Name}:{s.ParameterType.ToLuaType()}");
                var parstring = string.Join(",", parlist);
                var overload = $"{Comment("overload")} fun({parstring})";
                builder.AppendLine(overload);
            }

            var fulparlist = methodInfo.GetParameters().Select(s => s.Name);
            var fulparstring = string.Join(",", fulparlist);


            builder.AppendLine($"function {typename}.{methodName} ({fulparstring}) end");

            return builder.ToString();
        }

        private static (string Name, string Type) MapParameter(ParameterInfo arg)
        {
            var luaType = arg.ToLuaType();
            if (luaType.StartsWith("IEnumera"))
            {
                var genArgs = arg.ParameterType.GetGenericArguments()[0];
                luaType += $" | {genArgs.ToLuaType()}[]";
            }

            return (arg.Name, luaType);
        }

        private static string PropertyComment(PropertyInfo propertyInfo)
        {
            var start = $"{Comment("field")} public";
            var indexArray = $"{propertyInfo.Name} ";
            if (propertyInfo.GetIndexParameters().Length > 0)
            {
                var indexParams = string.Join(",",
                    propertyInfo.GetIndexParameters().Select(t => t.ParameterType.ToLuaType()));
                indexArray = $"[{indexParams}] ";
            }

            return $"{start} {indexArray}{propertyInfo.PropertyType.ToLuaType()}";
        }

        private static string ClassComment(TypeInfo typeInfo)
        {
            var luaType = typeInfo.ToLuaType();
            if (typeInfo == typeof(object)) luaType = "Object";

            var classComment = $"{Comment("class")} {luaType}";

            if (typeInfo.BaseType != null
                && typeInfo.BaseType != typeof(object)
                && typeInfo.IsVisibleToLua()
                && typeInfo.BaseType?.Assembly == typeInfo.Assembly)
                classComment += $" : {typeInfo.BaseType.ToLuaType()}";

            return classComment;
        }

        public static string Comment(string type)
        {
            return $"---@{type}";
        }
    }
}