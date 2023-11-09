using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Juntz.Serializator
{
    public static class Serializator
    {
        public static byte[] ToBytes<T>(T value)
        {
            var type = typeof(T);
            var layout = type.GetCustomAttribute<ByteLayoutAttribute>();
            if (layout == null)
                throw new ArgumentException("You must set the ByteLayout attribute to class or struct for serialization.");

            var buffer = new byte[layout.Length];
            foreach (var property in type.GetProperties())
            {
                var position = property.GetCustomAttribute<BytePositionAttribute>();
                if (position == null)
                {
                    // TODO: Automatically set the position information from a size of type
                    continue;
                }

                var bytes = GetBytes(property.PropertyType, property.GetValue(value));
                Array.Copy(bytes, 0, buffer, position.Index, position.Length);
            }

            // TODO: Add conversion for fields

            return buffer;
        }

        private static byte[] GetBytes(Type type, object value)
        {
            // TODO: Check endianness

            if (!type.IsPrimitive)
                throw new ArgumentException("Conversion not supported for non-primitive types.");

            var method = typeof(BitConverter).GetMethod("GetBytes", new [] { type });
            return (byte[])method.Invoke(null, new [] { value });
        }
    }
}
