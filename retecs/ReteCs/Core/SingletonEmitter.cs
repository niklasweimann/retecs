namespace retecs.ReteCs.core
{
    public static class SingletonEmitter
    {
        private static Emitter _instance;
        private static readonly object Padlock = new object();

        public static Emitter Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ??= new Emitter();
                }
            }
        }
    }
}
