using System;
using System.Collections.Generic;
using System.Reflection;

namespace ShuHai
{
    using StringList = List<string>;

    public static class EnumTraits<T>
        where T : struct
    {
        public static readonly Type Type = typeof(T);

        public static int ValueCount => values.Count;

        public static IEnumerable<string> Names => names;

        public static IEnumerable<T> Values => values;

        public static string GetName(int index) { return names[index]; }

        public static T GetValue(int index) { return values[index]; }

        public static T GetValue(string name) { return nameToValue[name]; }

        public static bool IsValid(T value) { return valueToNames.ContainsKey(value); }

        private static readonly Dictionary<string, T> nameToValue = new Dictionary<string, T>();
        private static readonly Dictionary<T, StringList> valueToNames = new Dictionary<T, StringList>();
        private static readonly StringList names = new StringList();
        private static readonly List<T> values = new List<T>();

        static EnumTraits()
        {
            if (!Type.IsEnum)
                throw new TypeLoadException("Enum type expected");

            var nameToField = new Dictionary<string, FieldInfo>();
            foreach (var f in Type.GetFields())
            {
                if (f.IsLiteral && !f.IsInitOnly && !f.IsDefined(typeof(ObsoleteAttribute), false))
                    nameToField.Add(f.Name, f);
            }

            foreach (var name in Enum.GetNames(Type))
            {
                if (!nameToField.TryGetValue(name, out var field))
                    continue;
                var value = (T)field.GetRawConstantValue();
                names.Add(name);
                values.Add(value);
                nameToValue.Add(name, value);
                valueToNames.Add(value, name);
            }
        }
    }
}