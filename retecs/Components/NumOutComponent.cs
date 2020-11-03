using System.Collections.Generic;
using System.Linq;
using retecs.ReteCs;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;

namespace retecs.Components
{
    public class NumOutComponent : Component 
    {
        public Emitter Emitter { get; }

        public NumOutComponent(Emitter emitter) : base("Number Output")
        {
            Emitter = emitter;
        }

        public override void Worker(NodeData node, Dictionary<string, List<object>> inputs, Dictionary<string, object> outputs, params object[] args)
        {
            Emitter.OnInfo("NodeInput: ", inputs);
            var input = inputs["innum"];
            node.Data.TryGetValue("innum", out var nodeData);
            var editorNode = Editor.Nodes.FirstOrDefault(x => x.Id == node.Id);
            if (editorNode == null)
            {
                Emitter.OnWarn("Node could not be found. Id was: " + node.Id);
                return;
            }

            const string controlKey = "num";
            var ctrl = editorNode.Controls[controlKey];
            if (input != null && input.Any())
            {
                ((NumControl)ctrl)?.SetValue(input.FirstOrDefault());
            }
            else if (nodeData != null)
            {
                ((NumControl)ctrl)?.SetValue(nodeData);
            }

        }

        public override void Builder(Node node)
        {
            var in1 = new Input("innum", "Number", Sockets.NumberSocket);
            in1.AddControl(new NumControl(Emitter, "innum"));
            node.AddControl(new NumControl(Emitter, "num")
            {
                Readonly = true
            }).AddInput(in1);
        }
    }
}