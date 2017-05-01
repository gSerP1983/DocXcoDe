using System;

namespace DocXcoDe.Extension
{
    public static class TypeExt
    {
        public static Type GetNullableUnderlyingType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return Nullable.GetUnderlyingType(type);

            return type;
        }
    }
}