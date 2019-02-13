using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ShuHai.VSFiles.Projects.Elements
{
    using StringPair = KeyValuePair<string, string>;

    public sealed class Conditions
        : IReadOnlyDictionary<string, string>, IReadOnlyList<Condition>, IEquatable<Conditions>
    {
        public int Count => list.Count;

        public Condition this[int index] => list[index];

        public string this[string key] => dict[key];

        public IEnumerable<string> Keys => dict.Keys;
        public IEnumerable<string> Values => dict.Values;

        #region Constructors

        public Conditions() : this((IEnumerable<Condition>)null) { }

        public Conditions(IEnumerable<StringPair> conditions)
            : this(conditions.Select(p => new Condition(p.Key, p.Value))) { }

        public Conditions(IEnumerable<Condition> conditions)
        {
            var dict = new Dictionary<string, string>();
            var list = new List<Condition>();
            if (conditions != null)
            {
                foreach (var condition in conditions)
                {
                    list.Add(condition);
                    dict.Add(condition.Name, condition.Value);
                }
            }
            this.list = list;
            this.dict = dict;

            namesString = new Lazy<string>(AppendNames(new StringBuilder(), false).ToString);
            valuesString = new Lazy<string>(AppendValues(new StringBuilder(), false).ToString);
            str = new Lazy<string>(BuildString);
            hashCode = HashCode.Get(list);
        }

        #endregion Constructors

        public bool Contains(Condition condition) => list.Contains(condition);

        public bool ContainsKey(string key) => dict.ContainsKey(key);

        public bool TryGetValue(string key, out string value) => dict.TryGetValue(key, out value);

        public IEnumerator<Condition> GetEnumerator() => list.GetEnumerator();

        private readonly IReadOnlyDictionary<string, string> dict;
        private readonly IReadOnlyList<Condition> list;

        #region Parse

        public static bool TryParse(string text, out Conditions conditions)
        {
            bool succeed = false;
            try
            {
                conditions = Parse(text);
                succeed = true;
            }
            catch (Exception)
            {
                conditions = null;
            }
            return succeed;
        }

        public static Conditions Parse(string text) => new Conditions(ParseAndEnumerate(text));

        public static IEnumerable<Condition> ParseAndEnumerate(string text)
        {
            var match = conditionRegex.Match(text);
            if (!match.Success)
                throw new ArgumentException("Invalid format of condition text.", nameof(text));

            var names = match.Groups["Names"].Value.Split('|');
            var values = match.Groups["Values"].Value.Split('|');
            int count = names.Length;
            if (count != values.Length)
                throw new ArgumentException("Number of configuration and its value does not match.", nameof(text));

            for (int i = 0; i < count; ++i)
            {
                var nameMatch = configurationNameRegex.Match(names[i]);
                if (!nameMatch.Success)
                    throw new ArgumentException("Invalid format of configuration name.", nameof(text));
                yield return new Condition(nameMatch.Groups["Name"].Value, values[i]);
            }
        }

        private static readonly Regex conditionRegex = new Regex(@"\'(?<Names>.+)\'\s*==\s*\'(?<Values>.*)\'");
        private static readonly Regex configurationNameRegex = new Regex(@"\$\((?<Name>\w+)\)");

        #endregion Parse

        #region Strings

        public string NamesString => namesString.Value;
        public string ValuesString => valuesString.Value;

        public override string ToString() => str.Value;

        [NonSerialized] private readonly Lazy<string> namesString;
        [NonSerialized] private readonly Lazy<string> valuesString;
        [NonSerialized] private readonly Lazy<string> str;

        private string BuildString()
        {
            var builder = new StringBuilder();

            builder.Append(' ');

            AppendNames(builder, true);
            builder.Append(" == ");
            AppendValues(builder, true);

            builder.Append(' ');

            return builder.ToString();
        }

        private StringBuilder AppendNames(StringBuilder builder, bool quote)
        {
            if (quote)
                builder.Append('\'');

            foreach (var condition in list)
                builder.Append($@"$({condition.Name})").Append('|');
            builder.RemoveTail(1);

            if (quote)
                builder.Append('\'');

            return builder;
        }

        private StringBuilder AppendValues(StringBuilder builder, bool quote)
        {
            if (quote)
                builder.Append('\'');

            foreach (var condition in list)
                builder.Append(condition.Value).Append('|');
            builder.RemoveTail(1);

            if (quote)
                builder.Append('\'');

            return builder;
        }

        #endregion Strings

        #region Equality

        public static bool operator ==(Conditions l, Conditions r)
            => EqualityComparer<Conditions>.Default.Equals(l, r);

        public static bool operator !=(Conditions l, Conditions r)
            => !EqualityComparer<Conditions>.Default.Equals(l, r);

        public bool Equals(Conditions other) { return list.SequenceEqual(other.list); }

        public override bool Equals(object obj) { return obj is Conditions conditions && Equals(conditions); }

        public override int GetHashCode() => hashCode;

        [NonSerialized] private readonly int hashCode;

        #endregion Equality

        #region Explicit Implementations

        IEnumerator<StringPair> IEnumerable<StringPair>.GetEnumerator() { return dict.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return list.GetEnumerator(); }

        #endregion Explicit Implementations
    }
}