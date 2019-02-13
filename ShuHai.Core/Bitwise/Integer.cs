namespace ShuHai.Bitwise
{
    public static class Integer
    {
        #region sbyte

        public static bool HasFlag(this sbyte self, sbyte flag) { return (self & flag) == flag; }
        public static bool HasFlag(this sbyte self, byte flag) { return (self & flag) == flag; }

        #endregion

        #region byte

        public static bool HasFlag(this byte self, sbyte flag) { return (self & flag) == flag; }
        public static bool HasFlag(this byte self, byte flag) { return (self & flag) == flag; }

        #endregion

        #region short

        public static bool HasFlag(this short self, sbyte flag) { return (self & flag) == flag; }
        public static bool HasFlag(this short self, short flag) { return (self & flag) == flag; }
        public static bool HasFlag(this short self, byte flag) { return (self & flag) == flag; }
        public static bool HasFlag(this short self, ushort flag) { return (self & flag) == flag; }

        #endregion

        #region ushort

        public static bool HasFlag(this ushort self, sbyte flag) { return (self & flag) == flag; }
        public static bool HasFlag(this ushort self, short flag) { return (self & flag) == flag; }
        public static bool HasFlag(this ushort self, byte flag) { return (self & flag) == flag; }
        public static bool HasFlag(this ushort self, ushort flag) { return (self & flag) == flag; }

        #endregion

        #region int

        public static bool HasFlag(this int self, sbyte flag) { return (self & flag) == flag; }
        public static bool HasFlag(this int self, short flag) { return (self & flag) == flag; }
        public static bool HasFlag(this int self, int flag) { return (self & flag) == flag; }
        public static bool HasFlag(this int self, byte flag) { return (self & flag) == flag; }
        public static bool HasFlag(this int self, ushort flag) { return (self & flag) == flag; }
        public static bool HasFlag(this int self, uint flag) { return (self & flag) == flag; }

        #endregion

        #region uint

        public static bool HasFlag(this uint self, sbyte flag) { return (self & flag) == flag; }
        public static bool HasFlag(this uint self, short flag) { return (self & flag) == flag; }
        public static bool HasFlag(this uint self, int flag) { return (self & flag) == flag; }
        public static bool HasFlag(this uint self, byte flag) { return (self & flag) == flag; }
        public static bool HasFlag(this uint self, ushort flag) { return (self & flag) == flag; }
        public static bool HasFlag(this uint self, uint flag) { return (self & flag) == flag; }

        #endregion

        #region long

        public static bool HasFlag(this long self, sbyte flag) { return (self & flag) == flag; }
        public static bool HasFlag(this long self, short flag) { return (self & flag) == flag; }
        public static bool HasFlag(this long self, int flag) { return (self & flag) == flag; }
        public static bool HasFlag(this long self, long flag) { return (self & flag) == flag; }

        public static bool HasFlag(this long self, byte flag) { return (self & flag) == flag; }
        public static bool HasFlag(this long self, ushort flag) { return (self & flag) == flag; }
        public static bool HasFlag(this long self, uint flag) { return (self & flag) == flag; }

        public static bool HasFlag(this long self, ulong flag)
        {
            var sflag = unchecked((long)flag);
            return (self & sflag) == sflag;
        }

        #endregion

        #region ulong

        public static bool HasFlag(this ulong self, sbyte flag)
        {
            var uflag = unchecked((byte)flag);
            return (self & uflag) == uflag;
        }

        public static bool HasFlag(this ulong self, short flag)
        {
            var uflag = unchecked((ushort)flag);
            return (self & uflag) == uflag;
        }

        public static bool HasFlag(this ulong self, int flag)
        {
            var uflag = unchecked((uint)flag);
            return (self & uflag) == uflag;
        }

        public static bool HasFlag(this ulong self, long flag)
        {
            var uflag = unchecked((ulong)flag);
            return (self & uflag) == uflag;
        }

        public static bool HasFlag(this ulong self, byte flag) { return (self & flag) == flag; }
        public static bool HasFlag(this ulong self, ushort flag) { return (self & flag) == flag; }
        public static bool HasFlag(this ulong self, uint flag) { return (self & flag) == flag; }
        public static bool HasFlag(this ulong self, ulong flag) { return (self & flag) == flag; }

        public static ulong AddFlag(this ulong self, sbyte flag) { return self | unchecked((byte)flag); }
        public static ulong AddFlag(this ulong self, short flag) { return self | unchecked((ushort)flag); }
        public static ulong AddFlag(this ulong self, int flag) { return self | unchecked((uint)flag); }
        public static ulong AddFlag(this ulong self, long flag) { return self | unchecked((ulong)flag); }
        public static ulong AddFlag(this ulong self, byte flag) { return self | flag; }
        public static ulong AddFlag(this ulong self, ushort flag) { return self | flag; }
        public static ulong AddFlag(this ulong self, uint flag) { return self | flag; }
        public static ulong AddFlag(this ulong self, ulong flag) { return self | flag; }

        public static ulong RemoveFlag(this ulong self, sbyte flag) { return self & unchecked((byte)~flag); }
        public static ulong RemoveFlag(this ulong self, short flag) { return self & unchecked((ushort)~flag); }
        public static ulong RemoveFlag(this ulong self, int flag) { return self & unchecked((uint)~flag); }
        public static ulong RemoveFlag(this ulong self, long flag) { return self & unchecked((ulong)~flag); }
        public static ulong RemoveFlag(this ulong self, byte flag) { return self & unchecked((byte)~flag); }
        public static ulong RemoveFlag(this ulong self, ushort flag) { return self & unchecked((ushort)~flag); }
        public static ulong RemoveFlag(this ulong self, uint flag) { return self & ~flag; }
        public static ulong RemoveFlag(this ulong self, ulong flag) { return self & ~flag; }

        #endregion
    }
}