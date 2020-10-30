using System.Collections.Generic;

namespace retecs.ReteCs.Entities
{
    public class Data
    {
        public string Id { get; set; }
        public Dictionary<string, NodeData>  Nodes { get; set; } = new Dictionary<string, NodeData>();
    }
}