namespace retecs.ReteCs.core
{
    public static class SingletonEmitter
    {
        private static Emitter _instance;

        public static Emitter Instance => _instance ??= new Emitter();
    }
}