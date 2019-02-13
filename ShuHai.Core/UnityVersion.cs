using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ShuHai
{
    [Serializable]
    public sealed class UnityVersion : IEquatable<UnityVersion>, IComparable<UnityVersion>
    {
        #region Version Parts

        public enum Type
        {
            Alpha,
            Beta,
            ReleaseCandidate,
            Final,
            Patch
        }

        /// <summary>
        ///     Cycle number of the version. For example, 5 in Unity-5.4.1 or 2017 in Unity-2017.2.1.
        /// </summary>
        public readonly ushort Cycle;

        /// <summary>
        ///     Major number of the version, or <see langword="null" /> to indicates it doesn't exist. For example,
        ///     4 in Unity-5.4.1 or 2 in Unity-2017.2.1.
        /// </summary>
        public readonly byte? Major;

        /// <summary>
        ///     Minor number of the version, or <see langword="null" /> to indicates it doesn't exist. For example,
        ///     1 in Unity-5.4.1 or 1 in Unity-2017.2.1.
        /// </summary>
        public readonly byte? Minor;

        /// <summary>
        ///     Release type of the version, or <see langword="null" /> to indicates it doesn't exist. For example, 'f' in
        ///     Unity-5.6.2f1 that stands for <see cref="Type.Final" /> or 'p' in Unity-2017.2.3p4 that stands for
        ///     <see cref="Type.Patch" />.
        /// </summary>
        public readonly Type? ReleaseType;

        /// <summary>
        ///     The version number comes after the release type word. For example, 4 in Unity-2017.2.3p4.
        /// </summary>
        public readonly byte ReleaseNumber;

        #endregion Version Parts

        #region Constructors

        /// <summary>
        ///     Initialize a new unity version object with specified version numbers.
        /// </summary>
        /// <param name="cycle"><see cref="Cycle" />.</param>
        /// <param name="major"><see cref="Major" />.</param>
        /// <param name="minor"><see cref="Minor" />.</param>
        /// <param name="releaseType"><see cref="ReleaseType" />.</param>
        /// <param name="releaseNumber"><see cref="ReleaseNumber" />.</param>
        public UnityVersion(ushort cycle, byte? major = null,
            byte? minor = null, Type? releaseType = null, byte releaseNumber = 0)
        {
            Cycle = cycle;
            Major = major;
            if (Major != null)
            {
                Minor = minor;
                if (Minor != null)
                {
                    ReleaseType = releaseType;
                    if (ReleaseType != null)
                        ReleaseNumber = releaseNumber;
                }
            }

            hashCode = HashCode.Get(Cycle, Major, Minor, ReleaseType, ReleaseNumber);
        }

        #endregion Constructors

        #region Parse

        public static bool TryParse(string version, out UnityVersion result)
        {
            Ensure.Argument.NotNull(version, nameof(version));

            try
            {
                result = ParseImpl(version);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        ///     Converts the string representation of unity version to its equivalent <see cref="UnityVersion" /> instance.
        /// </summary>
        /// <param name="version">The string representing the unity version.</param>
        /// <returns>An <see cref="UnityVersion" /> instance equivalent to <paramref name="version" />.</returns>
        public static UnityVersion Parse(string version)
        {
            Ensure.Argument.NotNull(version, nameof(version));
            return ParseImpl(version);
        }

        private static UnityVersion ParseImpl(string version)
        {
            version = version.Trim();

            var match = parseRegex.Match(version);
            if (!match.Success || match.Index != 0 || match.Length != version.Length)
                throw new ArgumentException("Invalid version string.", nameof(version));

            var groups = match.Groups;
            if (!ushort.TryParse(groups["Cycle"].Value, out ushort cycle))
                throw new ArgumentException("Invalid cycle in version string.", nameof(version));

            var major = ParseByteField(groups["Major"]);
            if (major == null)
                return new UnityVersion(cycle);

            var minor = ParseByteField(groups["Minor"]);
            if (minor == null)
                return new UnityVersion(cycle, major);

            var typeGroup = groups["Type"];
            var type = typeGroup.Success ? ParseType(typeGroup.Value) : null;
            if (type == null)
                return new UnityVersion(cycle, major, minor);

            var num = ParseByteField(groups["Num"]);
            if (num == null)
                throw new ArgumentException("Lack of release number in version string.", nameof(version));
            return new UnityVersion(cycle, major, minor, type, num.Value);
        }

        private static byte? ParseByteField(Group group)
        {
            if (!group.Success)
                return null;
            bool parsed = byte.TryParse(group.Value, out byte value);
            return parsed ? value : (byte?)null;
        }

        private static readonly Regex parseRegex = new Regex(
            $@"^((?i){Prefix}(?-i))*(?<Cycle>\d+)\.?(?<Major>\d*)\.?(?<Minor>\d*)(?<Type>[a-zA-Z]*)?(?<Num>\d*)$");

        public static Type? ParseType(string type)
        {
            if (string.IsNullOrEmpty(type))
                return null;
            if (TypeStringEquals(type, "f"))
                return Type.Final;
            if (TypeStringEquals(type, "p"))
                return Type.Patch;
            if (TypeStringEquals(type, "rc"))
                return Type.ReleaseCandidate;
            if (TypeStringEquals(type, "a"))
                return Type.Alpha;
            if (TypeStringEquals(type, "b"))
                return Type.Beta;
            return null;
        }

        private static bool TypeStringEquals(string type, string str)
        {
            return type.Equals(str, StringComparison.OrdinalIgnoreCase);
        }

        #endregion Parse

        #region To String

        private const string Prefix = "Unity-";

        /// <summary>
        ///     Converts current instance to its equivalent string representation.
        /// </summary>
        /// <param name="numberOnly">Determines whether to ommit the "Unity-" prefix string.</param>
        /// <returns>The string representation of current instance.</returns>
        public string ToString(bool numberOnly)
        {
            string prefix = numberOnly ? string.Empty : Prefix;
            if (ReleaseType == null)
            {
                if (Minor == null)
                {
                    if (Major == null)
                        return $"{prefix}{Cycle}";
                    return $"{prefix}{Cycle}.{Major}";
                }
                return $"{prefix}{Cycle}.{Major}.{Minor}";
            }
            return $"{prefix}{Cycle}.{Major}.{Minor}{TypeToString(ReleaseType.Value)}{ReleaseNumber}";
        }

        public override string ToString() { return ToString(false); }

        public static string TypeToString(Type type)
        {
            switch (type)
            {
                case Type.Alpha:
                    return "a";
                case Type.Beta:
                    return "b";
                case Type.ReleaseCandidate:
                    return "rc";
                case Type.Final:
                    return "f";
                case Type.Patch:
                    return "p";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        #endregion To String

        #region Equality

        /// <summary>
        ///     Check whether specified version object shared the same cycle value with current instance.
        /// </summary>
        /// <param name="other">The <see cref="UnityVersion" /> object to check.</param>
        /// <returns>
        ///     <see langword="true" /> if <see cref="Cycle" /> part of current instance is the same as that of the
        ///     <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool CycleEquals(UnityVersion other)
        {
            if (ReferenceEquals(this, other))
                return true;
            if (ReferenceEquals(other, null))
                return false;
            return Cycle == other.Cycle;
        }

        /// <summary>
        ///     Check whether specified version object shared the same cycle and major value with current instance.
        /// </summary>
        /// <param name="other">The <see cref="UnityVersion" /> object to check.</param>
        /// <returns>
        ///     <see langword="true" /> if <see cref="Cycle" /> and <see cref="Major" /> part of current instance is the same
        ///     as that of the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool MajorEquals(UnityVersion other)
        {
            return CycleEquals(other)
                && Nullable.Equals(Major, other.Major);
        }

        /// <summary>
        ///     Check whether specified version object shared the same cycle, major and minor value with current instance.
        /// </summary>
        /// <param name="other">The <see cref="UnityVersion" /> object to check.</param>
        /// <returns>
        ///     <see langword="true" /> if <see cref="Cycle" />, <see cref="Major" /> and <see cref="Minor" /> part of current
        ///     instance is the same as that of the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool MinorEquals(UnityVersion other)
        {
            return MajorEquals(other)
                && Nullable.Equals(Minor, other.Minor);
        }

        /// <summary>
        ///     Check whether specified version object represents the same value of current instance.
        /// </summary>
        /// <param name="other">The <see cref="UnityVersion" /> object to check.</param>
        /// <returns>
        ///     <see langword="true" /> if every part of current instance is the same as that of the <paramref name="other" />
        ///     parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(UnityVersion other)
        {
            return MinorEquals(other)
                && Nullable.Equals(ReleaseType, other.ReleaseType)
                && ReleaseNumber == other.ReleaseNumber;
        }

        public override bool Equals(object obj) { return Equals(obj as UnityVersion); }

        #endregion Equality

        #region Comparison

        public int CompareTo(UnityVersion other)
        {
            if (ReferenceEquals(this, other))
                return 0;
            if (ReferenceEquals(other, null))
                return 1;

            int cycleComparison = Cycle.CompareTo(other.Cycle);
            if (cycleComparison != 0)
                return cycleComparison;
            int majorComparison = Nullable.Compare(Major, other.Major);
            if (majorComparison != 0)
                return majorComparison;
            int minorComparison = Nullable.Compare(Minor, other.Minor);
            if (minorComparison != 0)
                return minorComparison;
            int releaseTypeComparison = Nullable.Compare(ReleaseType, other.ReleaseType);
            if (releaseTypeComparison != 0)
                return releaseTypeComparison;
            return ReleaseNumber.CompareTo(other.ReleaseNumber);
        }

        public override int GetHashCode() { return hashCode; }

        [NonSerialized] private readonly int hashCode;

        #endregion Comparison

        #region Operators

        public static bool operator ==(UnityVersion l, UnityVersion r) { return equalityComparer.Equals(l, r); }
        public static bool operator !=(UnityVersion l, UnityVersion r) { return !equalityComparer.Equals(l, r); }

        public static bool operator >(UnityVersion l, UnityVersion r) { return comparer.Compare(l, r) > 0; }
        public static bool operator <(UnityVersion l, UnityVersion r) { return comparer.Compare(l, r) < 0; }
        public static bool operator >=(UnityVersion l, UnityVersion r) { return comparer.Compare(l, r) >= 0; }
        public static bool operator <=(UnityVersion l, UnityVersion r) { return comparer.Compare(l, r) <= 0; }

        private static EqualityComparer<UnityVersion> equalityComparer => EqualityComparer<UnityVersion>.Default;
        private static Comparer<UnityVersion> comparer => Comparer<UnityVersion>.Default;

        #endregion Operators

        #region Part Versions

        /// <summary>
        ///     Get a version instance only contains <see cref="Cycle" /> part.
        /// </summary>
        public UnityVersion CycleVersion => Major == null ? this : new UnityVersion(Cycle);

        /// <summary>
        ///     Get a version instance only contains <see cref="Cycle" /> and <see cref="Major" /> part.
        /// </summary>
        public UnityVersion MajorVersion => Minor == null ? this : new UnityVersion(Cycle, Major);

        /// <summary>
        ///     Get a version instance only contains <see cref="Cycle" />, <see cref="Major" /> and <see cref="Minor" /> part.
        /// </summary>
        public UnityVersion MinorVersion => ReleaseType == null ? this : new UnityVersion(Cycle, Major, Minor);

        #endregion Part Versions
    }
}