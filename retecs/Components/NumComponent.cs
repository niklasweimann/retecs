using System.Collections.Generic;
using retecs.ReteCs;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;

namespace retecs.Components
{
    public class NumComponent : Component 
    {
        public Emitter Emitter { get; set; }

        public NumComponent(Emitter emitter) : base("Number Input")
        {
            Emitter = emitter;
        }

        public override void Worker(NodeData node, Dictionary<string, List<WorkerOutput>> inputs, Dictionary<string, WorkerOutput> outputs, params object[] args)
        {
            outputs["num"].Objects["num"] = node.Data["num"];
        }

        public override void Builder(Node node)
        {
            var out1 = new Output("num", "Number", Sockets.NumberSocket);
            node.AddControl(new NumControl(Emitter, "num")).AddOutput(out1);
        }
    }
}