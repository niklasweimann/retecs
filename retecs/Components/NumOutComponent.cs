using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using retecs.RenderPlugin.Entities;
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

        public override void Worker(NodeData node, Dictionary<string, List<WorkerOutput>> inputs, Dictionary<string, WorkerOutput> outputs, params object[] args)
        {
            Console.WriteLine("NodeInput: " + JsonSerializer.Serialize(inputs));
            var input = inputs["innum"] ?? new List<WorkerOutput>();
            var ctrl = Editor.Nodes.FirstOrDefault(x => x.Id == node.Id)?.Controls["num"];
            ctrl?.PutData("num", input);
        }

        public override void Builder(Node node)
        {
            var in1 = new Input("innum", "Number", Sockets.NumberSocket);
            node.AddControl(new NumControl(Emitter, "num")
            {
                Readonly = true
            }).AddInput(in1);
        }
    }
}