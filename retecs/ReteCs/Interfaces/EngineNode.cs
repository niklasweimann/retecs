using System;
using System.Collections.Generic;

namespace retecs.ReteCs.Interfaces
{
    public class EngineNode : NodeData
    {
        public bool Busy { get; set; }
        public List<Action> UnlockPool { get; set; }
        public Dictionary<string, WorkerOutput>  OutputData { get; set; }
    }
}