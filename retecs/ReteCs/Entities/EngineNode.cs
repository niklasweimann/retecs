using System;
using System.Collections.Generic;

namespace retecs.ReteCs.Entities
{
    public class EngineNode : NodeData
    {
        public bool Busy { get; set; }
        public List<Action> UnlockPool { get; set; }
        public Dictionary<string, object>  OutputData { get; set; }

        public EngineNode(NodeData nodeData)
        {
            Id = nodeData.Id;
            Name = nodeData.Name;
            Inputs = nodeData.Inputs;
            Outputs = nodeData.Outputs;
            Controls = nodeData.Controls;
            Data = nodeData.Data;
            Position = nodeData.Position;
        }
    }
}