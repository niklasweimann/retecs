using System;
using System.Text.Json;
using retecs.ReteCs.core;

namespace retecs.ReteCs
{
    public abstract class Control
    {
        public string Key { get; set; }
        public object Data { get; set; }
        public Node ParentNode { get; set; }
        public Input ParentInput { get; set; }
        public Emitter Emitter { get; set; }


        public Control(string key, Emitter emitter)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            Key = key;
            Emitter = emitter;
        }

        public Node GetNode()
        {
            return ParentNode ?? ParentInput?.Node ?? throw new Exception("Control hasn\'t be added to Input or Node");
        }

        public object GetData(string key)
        {
            return GetNode().Data[key];
        }

        public void PutData(string key, object data)
        {
            var node = GetNode();
            Emitter.OnInfo($"Writing to Node: {node.Name} with ID: {node.Id}");
            node.Data[key] = data;
            Emitter.OnInfo($"Value {JsonSerializer.Serialize(node.Data[key])} has been written to {key} ");
        }
    }
}