using System;
using System.Collections.Generic;

namespace retecs.ReteCs
{
    public class Selected
    {
        private List<Node> List { get; set; } = new List<Node>();

        public void Add(Node item, bool accumulate = false)
        {
            if (!accumulate)
            {
                List = new List<Node> {item};
            }
            else if(!Contains(item))
            {
                List.Add(item);
            }
        }

        public void Clear() => List.Clear();
        
        public void Remove(Node item) => List.Remove(item);
        public bool Contains(Node item) => List.Contains(item);
        public void Each(Action<Node> action) => List.ForEach(action);
    }
}