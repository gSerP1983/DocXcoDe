using System;
using System.ComponentModel;
using DocXcoDe.Extension;

namespace DocXcoDe.Util
{
    public static class SafeConverter
    {
        public static object Convert(object value, Type desiredType, object defaultValue = null)
        {
            return CoerceValue(desiredType, value, defaultValue);
        }

        public static T Convert<T>(object value)
        {
            return (T)CoerceValue(typeof(T), value, default(T));
        }

        private static object CoerceValue(Type desiredType, object value, object defaultValue)
        {
            if (value == null)
                return defaultValue;

            var valueType = value.GetType();
            if (desiredType.IsAssignableFrom(valueType))
                return value;

            if (desiredType.IsGenericType)
            {
                if (desiredType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    if (valueType == typeof(string) && string.IsNullOrEmpty(value.ToString()))
                        return null;
            }

            desiredType = desiredType.GetNullableUnderlyingType();

            try
            {
                if (desiredType == typeof(string))
                    return value.ToString();

                if (desiredType == typeof(bool) && Equals(value, "0"))
                    return false;

                if (desiredType == typeof(bool) && Equals(value, "1"))
                    return true;

                return System.Convert.ChangeType(value, desiredType);
            }
            catch
            {
                var cnv = TypeDescriptor.GetConverter(desiredType);
                if (cnv.CanConvertFrom(valueType))
                    return cnv.ConvertFrom(value);

                return value;
            }
        }
    }
}