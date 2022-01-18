using System.Linq;
using System.Reflection;

namespace CardGameEngine.LuaDocGen
{
    internal static class Visibility
    {
        public static bool IsVisibleToLua(TypeInfo arg)
        {
            return !Blacklist.IsBlacklisted(arg) && arg.IsVisible;
        }

        public static bool IsVisibleToLua(PropertyInfo arg)
        {
            if (arg.IsSpecialName) return false;
            if (Blacklist.IsBlacklisted(arg)) return false;
            var hasMoonSharp = arg.CustomAttributes.FirstOrDefault(f =>
                f.AttributeType.Name == "MoonSharpVisibleAttribute");
            if (hasMoonSharp != null)
                return IsVisibleToLua(arg.PropertyType.GetTypeInfo()) &&
                       (hasMoonSharp.ConstructorArguments[0].Value as bool? ?? false);
            return IsVisibleToLua(arg.PropertyType.GetTypeInfo()) && arg.GetAccessors().Any();
        }

        public static bool IsVisibleToLua(MethodInfo arg)
        {
            if (arg.IsSpecialName) return false;
            if (Blacklist.IsBlacklisted(arg)) return false;
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
    }
}