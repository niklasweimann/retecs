namespace retecs.ReteCs.core
{
    public abstract class Plugin
    {
        public abstract string Name { get; }

        public abstract void Install<T>(Context context, PluginParams<T> pluginParams) where T : Plugin;
    }
}