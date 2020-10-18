using System.Collections.Generic;

namespace retecs.ReteCs.Interfaces
{
    public class NodeData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, IInputData> Inputs { get; set; }
        public Dictionary<string, IOutputData>  Outputs { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public (int X, int Y) Position { get; set; }
    }
}