using System;
using System.Linq;
using System.Reflection;

namespace Juntz.Serializator
{
    public static class Serializator
    {
        public static byte[] ToBytes<T>(T value)
        {
            var type = typeof(T);
            var layout = GetLayout(type);
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

        public static T FromBytes<T>(byte[] bytes) where T: new()
        {
            var result = new T();
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                var position = property.GetCustomAttribute<BytePositionAttribute>();
                if (position == null)
                {
                    continue;
                }

                var valueBytes = bytes.Skip(position.Index).Take(position.Length).ToArray();
                var value = ToValue(property.PropertyType, valueBytes);
                property.SetValue(result, value);
            }

            return result;
        }

        private static ByteLayoutAttribute GetLayout(Type type)
        {
            var layout = type.GetCustomAttribute<ByteLayoutAttribute>();
            if (layout == null)
                throw new ArgumentException("You must set the ByteLayout attribute to class or struct for serialization.");
            return layout;
        }

        private static byte[] GetBytes(Type type, object value)
        {
            // TODO: Check endianness

            if (!type.IsPrimitive)
                throw new ArgumentException("Conversion not supported for non-primitive types.");

            var method = typeof(BitConverter).GetMethod("GetBytes", new [] { type });
            return (byte[])method.Invoke(null, new [] { value });
        }

        public static object ToValue(Type type, byte[] bytes)
        {
            var methodName = "To" + type.Name;
            var argumentTypes = new [] { typeof(byte[]), typeof(int) };
            var method = typeof(BitConverter).GetMethod(methodName, argumentTypes);
            return method.Invoke(null, new object [] { bytes, 0 });
        }
    }
}
