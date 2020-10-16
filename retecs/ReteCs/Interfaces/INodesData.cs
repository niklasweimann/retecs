using System.Collections.Generic;

namespace retecs.ReteCs.Interfaces
{
    public interface INodesData
    {
        public Dictionary<string, INodeData> Nodes { get; set; }
    }
}