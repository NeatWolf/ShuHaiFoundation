using System;
using System.Collections.Generic;

namespace ShuHai.VSFiles.Projects.Elements
{
    using StringPair = KeyValuePair<string, string>;

    public struct Condition : IEquatable<Condition>
    {
        public readonly string Name;
        public readonly string Value;

        #region Constructors

        public Condition(string name, string value)
        {
            Name = name;
            Value = value;
            hashCode = HashCode.Get(Name, Value);
        }

        public Condition(StringPair condition)
        {
            Name = condition.Key;
            Value = condition.Value;
            hashCode = HashCode.Get(Name, Value);
        }

        #endregion Constructors

        #region Equality

        public static bool operator ==(Condition l, Condition r)
            => EqualityComparer<Condition>.Default.Equals(l, r);

        public static bool operator !=(Condition l, Condition r)
            => !EqualityComparer<Condition>.Default.Equals(l, r);

        public bool Equals(Condition other) => string.Equals(Name, other.Name) && string.Equals(Value, other.Value);

        public override bool Equals(object obj) => obj is Condition condition && Equals(condition);

        public override int GetHashCode() => hashCode;

        [NonSerialized] private readonly int hashCode;

        #endregion Equality

        public static class Names
        {
            public const string Configuration = "Configuration";
            public const string Platform = "Platform";
        }
    }
}