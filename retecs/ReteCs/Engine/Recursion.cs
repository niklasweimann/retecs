using System.Collections.Generic;
using System.Linq;
using retecs.ReteCs.Interfaces;

namespace retecs.ReteCs.Engine
{
    public class Recursion
    {
        public Dictionary<string, NodeData> Nodes { get; set; }
        
        public Recursion(Dictionary<string, NodeData> nodes)
        {
            Nodes = nodes;
        }

        public List<NodeData> ExtractInputNodes(NodeData nodeData)
        {
            var res = new List<NodeData>();
            foreach (var nodeDataInput in nodeData.Inputs)
            {
                var connections = nodeData.Inputs[nodeDataInput.Key];
                var nodesData = new List<NodeData>();
                foreach (var connection in connections.Connections)
                {
                    // TODO b
                    nodesData.Add(Nodes[connection.Node]);
                }
                res.AddRange(nodesData);
                //TODO acc
            }

            return res;
        }

        public NodeData FindSelf(List<NodeData> list, List<NodeData> inputNodes)
        {
            var intersect = list.Intersect(inputNodes).ToList();
            if (intersect.Any())
            {
                return intersect.FirstOrDefault();
            }

            foreach (var node in inputNodes)
            {
                var l = new List<NodeData>{node};
                l.AddRange(list);
                var inter = FindSelf(l, ExtractInputNodes(node));

                if (inter != null)
                {
                    return inter;
                }
            }

            return null;
        }

        public NodeData Detect()
        {
            return Nodes.Values.Select(node => FindSelf(new List<NodeData> {node}, ExtractInputNodes(node)))
                .FirstOrDefault(inters => inters != null);
        }
    }
}