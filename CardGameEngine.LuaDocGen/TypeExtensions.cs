using System;
using System.Linq;
using System.Reflection;

namespace CardGameEngine.LuaDocGen
{
    internal static class TypeExtensions
    {
        public static string GetNameWithoutGenericArity(this Type t)
        {
            var name = t.Name;
            var index = name.IndexOf('`');
            return index == -1 ? name : name.Substring(0, index);
        }

        public static string ToLuaType(this ParameterInfo parameterInfo)
        {
            if (parameterInfo.HasDefaultValue && parameterInfo.DefaultValue == null)
                return $"({ToLuaType(parameterInfo.ParameterType)}) | nil";

            return parameterInfo.ParameterType.ToLuaType();
        }

        public static string ToLuaType(this Type propertyInfoPropertyType)
        {
            if (propertyInfoPropertyType == typeof(Type)) return "Type<Event>";
            if (propertyInfoPropertyType == typeof(object)) return "Object | any";

            if (propertyInfoPropertyType.IsGenericType)
            {
                //todo peut etre ajouter list pour avoir contains et tout a moins que moonsharp transforme en tableau
                //voir IEnumerable 
                // if (typeof(IEnumerable).IsAssignableFrom(propertyInfoPropertyType))
                //     return $"{ToLuaType(propertyInfoPropertyType.GetGenericArguments()[0])}[]";

                if (propertyInfoPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    return $"{ToLuaType(propertyInfoPropertyType.GetGenericArguments()[0])} | nil";


                var genArgs = string.Join(",", propertyInfoPropertyType.GetGenericArguments().Select(ToLuaType));
                return $"{GetNameWithoutGenericArity(propertyInfoPropertyType.GetGenericTypeDefinition())}<{genArgs}>";
            }

            if (propertyInfoPropertyType.IsEnum) return propertyInfoPropertyType.Name;

            if (IsNumericType(propertyInfoPropertyType.GetTypeInfo())) return "number";

            if (propertyInfoPropertyType == typeof(bool)) return "boolean";

            if (propertyInfoPropertyType == typeof(string)) return "string";

            return propertyInfoPropertyType.Name == "Closure" ? "fun()" : propertyInfoPropertyType.Name;
        }

        public static bool IsVisibleToLua(this TypeInfo t)
        {
            return Visibility.IsVisibleToLua(t);
        }

        private static bool IsNumericType(Type o)
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