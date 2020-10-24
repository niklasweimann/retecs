using System.Collections.Generic;

namespace retecs.ReteCs.Entities
{
    public class NodeData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, InputData> Inputs { get; set; }
        public Dictionary<string, OutputData>  Outputs { get; set; }
        public Dictionary<string, Control>  Controls { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public (int X, int Y) Position { get; set; }
    }
}