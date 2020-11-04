using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using retecs.ReteCs;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;

namespace retecs.Components
{
    public class TextOutComponent : Component
    {
        public Emitter Emitter { get; }

        public TextOutComponent(Emitter emitter) : base("Text Output") => Emitter = emitter;

        public override void Worker(
            NodeData node,
            Dictionary<string, List<object>> inputs,
            Dictionary<string, object> outputs,
            params object[] args)
        {
            inputs.TryGetValue("inText", out var input);
            var editorNode = Editor.Nodes.FirstOrDefault(n => n.Id == node.Id);
            Emitter.OnDebug($"editorNode is null? {editorNode == null}");
            if (editorNode == null)
            {
                return;
            }

            editorNode.Controls.TryGetValue("preview", out var control);
            Emitter.OnDebug($"control is null? {control == null}");
            ((TextControl) control)?.SetValue(input?.FirstOrDefault() ?? string.Empty);
        }

        public override void Builder(Node node)
        {
            var textOutput = new Input("inText", "Text", Sockets.TextSocket);
            node.AddControl(new TextControl(Emitter, "preview", true))
                .AddInput(textOutput);
        }
    }
}
