using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.Services
{
    public static class TypeExtensions
    {
        public static decimal ToDecimal(this object input)
        {
            if (IsNullValue(input))
                return 0;
            return Convert.ToDecimal(input);
        }

        public static bool ToBoolean(this object input)
        {
            if (IsNullValue(input))
                return false;
            return Convert.ToBoolean(input);
        }

        public static int ToInt(this object input)
        {
            if (IsNullValue(input))
                return 0;
            return Convert.ToInt32(input);
        }

        public static long ToLong(this object input)
        {
            if (IsNullValue(input))
                return 0;
            return Convert.ToInt64(input);
        }

        public static Guid ToGuid(this object input)
        {
            if (IsNullValue(input))
                return Guid.Empty;
            return Guid.Parse(input.ToString());
        }

        public static DateTime ToDateTime(this object input)
        {
            if (IsNullValue(input))
                return DateTime.MinValue;
            return Convert.ToDateTime(input);
        }

        public static float ToFloat(this object input)
        {
            if (IsNullValue(input))
                return 0;
            return Convert.ToSingle(input);
        }

        public static string ToDefaultString(this object input)
        {
            if (IsDBNullValue(input))
                return string.Empty;
            return Convert.ToString(input).Trim();
        }

        public static bool IsNullValue(object input)
        {
            return (input == null || input == DBNull.Value);
        }

        public static bool IsDBNullValue(object input)
        {
            return (input == DBNull.Value);
        }

        public static T ToEnumValue<T>(this object input) where T : struct, IConvertible
        {
            T result = default(T);
            if (typeof(T).IsEnum && !IsNullValue(input))
            {
                result = (T)Enum.Parse(typeof(T), input.ToDefaultString().ToLower(), true);
            }
            return result;
        }

        public static Guid? ToDBValue(this Guid input)
        {
            if (input == Guid.Empty)
                return null;
            else
                return (Guid?)input;
        }

        public static byte[] ToBytes(this object input)
        {
            if (IsDBNullValue(input))
                return default(byte[]);
            return (byte[])input;
        }

        public static TResult NotNull<TResult, TSource>(this TSource source, Func<TSource, TResult> onNotDefault) where TSource : class
        {
            if (onNotDefault == null) throw new ArgumentNullException("onNotDefault");

            return source == null ? default(TResult) : onNotDefault(source);
        }


        public static bool IsEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        public static bool IsNullorEmpty(this Guid guid)
        {
            if (guid == Guid.Empty || guid == null)
                return true;
            else
                return false;
        }

        public static bool IsNullorEmpty(this Guid? guid)
        {
            if (guid == Guid.Empty || guid == null)
                return true;
            else
                return false;
        }

        public static bool IsNullorMin(this DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue || dateTime == null)
                return true;
            else
                return false;
        }

        public static bool IsNullorMin(this DateTime? dateTime)
        {
            if (dateTime == DateTime.MinValue || dateTime == null)
                return true;
            else
                return false;
        }

        public static bool IsFutureDate(this DateTime dateTime)
        {
            if (dateTime > DateTime.Now)
                return true;
            else
                return false;
        }

        public static decimal ToRound(this decimal val, int len)
        {
            return Math.Round(val, len);
        }

        public static string[] ToArray(this Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return properties.Select(x => x.Name).ToArray();
        }
    }

}
