namespace ShuHai.Unity
{
    public static class Root
    {
        public static readonly SequentialActions Initialize = new SequentialActions();
        public static readonly SequentialActions Deinitialize = new SequentialActions();
        public static readonly SequentialActions Update = new SequentialActions();

        internal static void OnInitialize() { Initialize.Raise(); }
        internal static void OnDeinitialize() { Deinitialize.Raise(); }
        internal static void OnUpdate() { Update.Raise(); }
    }
}