using System;
using System.Linq;
using System.Reflection;

namespace CardGameEngine.LuaDocGen
{
    internal static class Blacklist
    {
        private static readonly string[] _blacklist =
        {
            "IPropertyChange", "Exception", "Effect", "EventManager", "IExternCallbacks",
            "IEnumerator"
        };

        public static bool IsBlacklisted(Type arg)
        {
            return typeof(Delegate).IsAssignableFrom(arg) || IsBlacklisted(arg.Name);
        }

        private static bool IsBlacklisted(string arg)
        {
            return _blacklist.Any(arg.Contains);
        }

        public static bool IsBlacklisted(PropertyInfo propertyInfo)
        {
            return IsBlacklisted(propertyInfo.Name) || IsBlacklisted(propertyInfo.PropertyType);
        }

        public static bool IsBlacklisted(MethodInfo methodInfo)
        {
            return IsBlacklisted(methodInfo.ReturnType) ||
                   methodInfo.GetParameters().Any(IsBlacklisted);
        }

        private static bool IsBlacklisted(ParameterInfo arg)
        {
            return IsBlacklisted(arg.ParameterType);
        }
    }
}