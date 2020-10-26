using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using retecs.ReteCs.Entities;

namespace retecs.ReteCs
{
    public class Node
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public Point Position { get; set; }
        public Dictionary<string, Input> Inputs { get; set; } = new Dictionary<string, Input>();
        public Dictionary<string, Output> Outputs { get; set; } = new Dictionary<string, Output>();
        public Dictionary<string, Control> Controls { get; set; } = new Dictionary<string, Control>();
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

        public event Action UpdateEvent;

        private static int latestId = 0;

        public Node(string name)
        {
            Name = name;
            Id = IncrementId().ToString();
        }

        public static int IncrementId()
        {
            return latestId++;
        }

        public static void ResetId()
        {
            latestId = 0;
        }

        private void Add<T>(Dictionary<string, T> list, T item) where T : Io
        {
            if (list.ContainsKey(item.Key))
            {
                throw new Exception($"Item with key {item.Key} already been added to the node");
            }

            if (item.Node != null)
            {
                throw new Exception("Item has already been added to some node");
            }

            item.Node = this;
            list[item.Key] = item;
        }
        private void AddControl<T>(Dictionary<string, T> list, T item) where T : Control
        {
            if (list.ContainsKey(item.Key))
            {
                throw new Exception($"Item with key {item.Key} already been added to the node");
            }

            if (item.ParentInput != null || item.ParentNode != null)
            {
                throw new Exception("Item has already been added to some node");
            }

            item.ParentNode = this;
            list[item.Key] = item;
        }

        public Node AddControl(Control control)
        {
            AddControl(Controls, control);
            return this;
        }

        public void RemoveControl(Control control)
        {
            control.ParentInput = null;
            control.ParentNode = null;
        }

        public Node AddInput(Input input)
        {
            Add(Inputs, input);
            return this;
        }

        public void RemoveInput(Input input)
        {
            input.RemoveAllConnections();
            input.Node = null;
            Inputs.Remove(input.Key);
        }

        public Node AddOutput(Output output)
        {
            Add(Outputs, output);
            return this;
        }

        public void RemoveOutput(Output output)
        {
            output.RemoveAllConnections();
            output.Node = null;

            Outputs.Remove(output.Key);
        }

        public List<Connection> GetConnections()
        {
            var ios = new List<Io>();
            ios.AddRange(Inputs.Values);
            ios.AddRange(Outputs.Values);
            var connections = ios.Aggregate(new List<Connection>(), (arr, io) =>
            {
                var list = new List<Connection>();
                list.AddRange(arr);
                list.AddRange(io.Connections);
                return list;
            });
            return connections;
        }

        public void Update()
        {
            UpdateEvent?.Invoke();
        }

        private Dictionary<string, InputData> ReduceIO(Dictionary<string, Input> list) =>
            list.ToDictionary(input => input.Key, input => input.Value.ToJson());
        
        private Dictionary<string, OutputData> ReduceIO(Dictionary<string, Output> list) =>
            list.ToDictionary(output => output.Key, output => output.Value.ToJson());

        public NodeData ToJson()
        {
            return new NodeData
            {
                Id = Id,
                Data = Data,
                Inputs = ReduceIO(Inputs),
                Outputs = ReduceIO(Outputs),
                Position = Position,
                Name = Name
            };
        }

        public static Node FromJson(NodeData nodeData)
        {
            var node = new Node(nodeData.Name)
            {
                Position = nodeData.Position, Id = nodeData.Id, Data = nodeData.Data, Name = nodeData.Name
            };
            latestId = Math.Max(Convert.ToInt32(node.Id), latestId);
            return node;
        }
    }
}