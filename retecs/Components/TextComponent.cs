using System.Collections.Generic;
using retecs.ReteCs;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;

namespace retecs.Components
{
    public class TextComponent : Component
    {
        public Emitter Emitter { get; }

        public TextComponent(Emitter emitter) : base("Text Input") => Emitter = emitter;

        public override void Worker(
            NodeData node,
            Dictionary<string, List<object>> inputs,
            Dictionary<string, object> outputs,
            params object[] args)
        {
            Emitter.OnDebug($"outputs is null?{outputs == null}");
            Emitter.OnDebug($"node.Data is null?{node?.Data == null}");
            outputs["string"] = node.Data["string"];
        }

        public override void Builder(Node node)
        {
            var textOutput = new Output("string", "Text", Sockets.TextSocket);
            node.AddControl(new TextControl(Emitter, "string"))
                .AddOutput(textOutput);
        }
    }
}
