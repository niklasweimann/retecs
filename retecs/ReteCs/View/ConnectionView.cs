using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using retecs.ReteCs.core;

namespace retecs.ReteCs.View
{
    public class ConnectionView : Emitter
    {
        public Connection Connection { get; set; }
        public NodeView InputNode { get; set; }
        public NodeView OutputNode { get; set; }
        public ElementReference HtmlElement { get; set; }

        public ConnectionView(Connection connection, NodeView inputNode, NodeView outputNode)
        {
            Connection = connection;
            InputNode = inputNode;
            OutputNode = outputNode;
            /*
             TODO
             * this.el = document.createElement('div');
        this.el.style.position = 'absolute';
        this.el.style.zIndex = '-1';
             */
            OnRenderConnection(HtmlElement, connection, GetPoints());
        }

        public ((double X, double Y),(double X, double Y)) GetPoints()
        {
            return (OutputNode.GetSocketPosition(Connection.Output), OutputNode.GetSocketPosition(Connection.Input));
        }

        public void Update()
        {
            OnUpdateConnection(HtmlElement, Connection, GetPoints());
        }
    }
}