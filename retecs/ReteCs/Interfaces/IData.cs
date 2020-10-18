using System.Collections.Generic;

namespace retecs.ReteCs.Interfaces
{
    public interface IData
    {
        public string Id { get; set; }
        public Dictionary<string, NodeData>  Nodes { get; set; }
    }
}