using System.Collections.Generic;

namespace retecs.ReteCs.Interfaces
{
    public interface INodeData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IInputData InputData { get; set; }
        public IOutputData OutputData { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public (int X, int Y) Position { get; set; }
    }
}