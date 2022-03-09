using System;
using System.Collections.Generic;
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

        private static readonly List<string> _whitelist = new List<string>
        {
            "CardEffectPlayEvent"
        };

        public static bool IsBlacklisted(Type arg)
        {
            return (typeof(Delegate).IsAssignableFrom(arg) || IsBlacklisted(arg.Name)) && !IsWhitelisted(arg);
        }

        public static bool IsWhitelisted(Type arg)
        {
            return IsWhitelisted(arg.Name);
        }

        private static bool IsBlacklisted(string arg)
        {
            return _blacklist.Any(arg.Contains) && !IsWhitelisted(arg);
        }

        private static bool IsWhitelisted(string arg)
        {
            return _whitelist.Any(arg.Contains);
        }

        public static bool IsBlacklisted(PropertyInfo propertyInfo)
        {
            return (IsBlacklisted(propertyInfo.Name) || IsBlacklisted(propertyInfo.PropertyType)) &&
                   !IsWhitelisted(propertyInfo);
        }

        public static bool IsWhitelisted(PropertyInfo propertyInfo)
        {
            return IsWhitelisted(propertyInfo.Name) && IsWhitelisted(propertyInfo.PropertyType);
        }

        public static bool IsBlacklisted(MethodInfo methodInfo)
        {
            return (IsBlacklisted(methodInfo.ReturnType) ||
                    methodInfo.GetParameters().Any(IsBlacklisted)) && !IsWhitelisted(methodInfo);
        }

        public static bool IsWhitelisted(MethodInfo methodInfo)
        {
            return IsWhitelisted(methodInfo.ReturnType) ||
                   methodInfo.GetParameters().Any(IsWhitelisted);
        }

        private static bool IsBlacklisted(ParameterInfo arg)
        {
            return IsBlacklisted(arg.ParameterType) && !IsWhitelisted(arg);
        }

        private static bool IsWhitelisted(ParameterInfo arg)
        {
            return IsWhitelisted(arg.ParameterType);
        }
    }
}