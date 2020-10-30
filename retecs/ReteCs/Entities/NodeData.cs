using System.Collections.Generic;

namespace retecs.ReteCs.Entities
{
    public class NodeData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, InputData> Inputs { get; set; } = new Dictionary<string, InputData>();
        public Dictionary<string, OutputData>  Outputs { get; set; } = new Dictionary<string, OutputData>();
        public Dictionary<string, Control>  Controls { get; set; } = new Dictionary<string, Control>();
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        public Point Position { get; set; }
    }
}