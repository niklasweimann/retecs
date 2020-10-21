using System.Collections.Generic;

namespace retecs.ReteCs.Interfaces
{
    public class Data
    {
        public string Id { get; set; }
        public Dictionary<string, NodeData>  Nodes { get; set; }
    }
}