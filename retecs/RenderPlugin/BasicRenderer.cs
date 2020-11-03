using retecs.ReteCs;
using retecs.ReteCs.core;

namespace retecs.RenderPlugin
{
    public class BasicRenderer : Plugin
    {
        public override string Name { get; } = "BasicRenderer";
        public Context Context { get; set; }

        public override void Install<T>(Context context, PluginParams<T> pluginParams)
        {
            Context = context;
            context.Emitter.ConnectionCreated += HandleRenderConnectionCreateAndRemoved;
            context.Emitter.ConnectionRemoved += HandleRenderConnectionCreateAndRemoved;
            context.Emitter.NodeSelected += HandleNodeSelected;
        }

        private void HandleRenderConnectionCreateAndRemoved(Connection connection)
        {
            connection.Output.Node.Update();
            connection.Input.Node.Update();
        }

        private void HandleNodeSelected(Node node)
        {
            ((NodeEditor)Context).Nodes.ForEach(n => n.Update());
        }
    }
}
