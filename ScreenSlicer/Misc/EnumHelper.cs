using System;

namespace ScreenSlicer
{
    public static class EnumHelper
    {
        public static T Add<T>(this Enum value, T add)
        {
            Type type = value.GetType();

            object result = value;
            var parsed = new _Value(add, type);
            if (parsed.Signed is long)
            {
                result = Convert.ToInt64(value) | (long)parsed.Signed;
            }
            else if (parsed.Unsigned is ulong)
            {
                result = Convert.ToUInt64(value) | (ulong)parsed.Unsigned;
            }

            return (T)Enum.ToObject(type, result);
        }

        public static Enum Add(this Enum value, Enum add)
        {
            return Add<Enum>(value, add);
        }

        public static T Remove<T>(this Enum value, T remove)
        {
            Type type = value.GetType();

            object result = value;
            var parsed = new _Value(remove, type);
            if (parsed.Signed is long)
            {
                result = Convert.ToInt64(value) & ~(long)parsed.Signed;
            }
            else if (parsed.Unsigned is ulong)
            {
                result = Convert.ToUInt64(value) & ~(ulong)parsed.Unsigned;
            }

            return (T)Enum.ToObject(type, result);
        }

        public static Enum Remove(this Enum value, Enum remove)
        {
            return Remove<Enum>(value, remove);
        }

        private class _Value
        {
            private static readonly Type _UInt32 = typeof(long);
            private static readonly Type _UInt64 = typeof(ulong);

            public readonly long? Signed;
            public readonly ulong? Unsigned;

            public _Value(object value, Type type)
            {
                if (!type.IsEnum)
                    throw new ArgumentException("Value provided is not an enumerated type!");

                var compare = Enum.GetUnderlyingType(type);

                if (compare.Equals(_UInt32) || compare.Equals(_UInt64))
                    Unsigned = Convert.ToUInt64(value);
                else
                    Signed = Convert.ToInt64(value);
            }
        }
    }
}
