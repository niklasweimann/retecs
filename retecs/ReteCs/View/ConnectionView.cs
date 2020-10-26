using Microsoft.AspNetCore.Components;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;

namespace retecs.ReteCs.View
{
    public class ConnectionView
    {
        private Emitter Emitter { get; set; }
        public Connection Connection { get; set; }
        public NodeView InputNode { get; set; }
        public NodeView OutputNode { get; set; }
        public ElementReference HtmlElement { get; set; }

        public ConnectionView(Connection connection, NodeView inputNode, NodeView outputNode, Emitter emitter)
        {
            Connection = connection;
            InputNode = inputNode;
            OutputNode = outputNode;
            Emitter = emitter;
            /*
             TODO
             * this.el = document.createElement('div');
        this.el.style.position = 'absolute';
        this.el.style.zIndex = '-1';
             */
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