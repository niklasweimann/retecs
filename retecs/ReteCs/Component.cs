using System.Collections.Generic;

namespace retecs.ReteCs
{
    public abstract class Component : Engine.Component
    {
        public NodeEditor Editor { get; set; }
        public object Data { get; set; }

        protected Component(string name) : base(name)
        {
        }

        public abstract void Builder(Node node);

        public Node Build(Node node)
        {
            Builder(node);
            return node;
        }

        public Node CreateNode(Dictionary<string, object> data)
        {
            var node = new Node(Name) {Data = data};
            Build(node);
            return node;
        }
    }
}