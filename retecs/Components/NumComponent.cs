using System.Collections.Generic;
using retecs.ReteCs;
using retecs.ReteCs.Entities;

namespace retecs.Components
{
    public class NumComponent : Component
    {
        public NumComponent() : base("Number Input")
        {
        }

        public override void Worker(NodeData node, Dictionary<string, List<WorkerOutput>> inputs, Dictionary<string, WorkerOutput> outputs, params object[] args)
        {
            outputs["num"].Objects["num"] = node.Data["num"];
        }

        public override void Builder(Node node)
        {
        }
    }
}