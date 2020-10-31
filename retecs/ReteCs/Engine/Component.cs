using System.Collections.Generic;
using retecs.ReteCs.Entities;

namespace retecs.ReteCs.Engine
{
    public abstract class Component
    {
        public string Name { get; set; }
        public object Data { get; set; }
        public Engine Engine { get; set; }

        public Component(string name)
        {
            Name = name;
        }

        public abstract void Worker(NodeData node,
            Dictionary<string, List<object>> inputs,
            Dictionary<string, object> outputs,
            params object[] args);
    }
}