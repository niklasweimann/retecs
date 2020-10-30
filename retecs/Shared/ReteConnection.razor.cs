using Microsoft.AspNetCore.Components;
using retecs.ReteCs;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;

namespace retecs.Shared
{
    public partial class ReteConnection
    {
        private Emitter Emitter { get; set; }
        public Connection Connection { get; set; }
        public ReteNode InputNode { get; set; }
        public ReteNode OutputNode { get; set; }
        public ElementReference HtmlElement { get; set; }

        public ReteConnection()
        {
            
        }
        public ReteConnection(Connection connection, ReteNode inputNode, ReteNode outputNode, Emitter emitter)
        {
            Connection = connection;
            InputNode = inputNode;
            OutputNode = outputNode;
            Emitter = emitter;
            Emitter.OnRenderConnection(HtmlElement, connection, GetPoints());
        }

        public (Point,Point) GetPoints()
        {
            return (OutputNode.GetSocketPosition(Connection.Output), OutputNode.GetSocketPosition(Connection.Input));
        }

        public void Update()
        {
            Emitter.OnUpdateConnection(HtmlElement, Connection, GetPoints());
        }
    }
}