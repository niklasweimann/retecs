using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using retecs.ReteCs;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;

namespace retecs.Shared
{
    public partial class ReteEditor
    {
        [Inject]
        public Emitter Emitter { get; set; }
        public ElementReference Container { get; set; }
        public Dictionary<string, Component> Components { get; set; }
        public Dictionary<Node, ReteNode> Nodes { get; } = new Dictionary<Node, ReteNode>();
        public Dictionary<Connection, ReteConnection> ConnectionViews { get; } = new Dictionary<Connection, ReteConnection>();
        public ReteArea Area { get; set; }

        public ReteEditor()
        {
            
        }

        public ReteEditor(ElementReference container, Dictionary<string, Component> components, Emitter emitter)
        {
            Emitter = emitter;
            Container = container;
            Components = components;
            Emitter.Click += (args, reference) => Click(args);
            Emitter.WindowContextMenu += args =>  Emitter.OnContextMenu(args);
            Emitter.WindowResize += Resize;
            Emitter.Destroy += () =>
            {
                Emitter.WindowResize -= Resize;
            };

            Emitter.NodeTranslated += UpdateConnections;
            Area = new ReteArea(Emitter);
        }

        public void AddNode(Node node)
        {
            Components.TryGetValue(node.Name, out var component);
            if (component == null)
            {
                throw new Exception($"Component {node.Name} not found");
            }
            var nodeView = new ReteNode(node, component, Emitter);
            Nodes.Add(node, nodeView);
            Area.AppendChild(nodeView.HtmlElement);
        }

        public void RemoveNode(Node node)
        {
            Nodes.TryGetValue(node, out var nodeView);
            Nodes.Remove(node);
            if (nodeView != null)
            {
                Area.RemoveChild(nodeView.HtmlElement);
            }
        }

        public void AddConnection(Connection connection)
        {
            if (connection.Input.Node == null || connection.Output.Node == null)
            {
                throw new Exception("Connection input or output not added to node");
            }

            Nodes.TryGetValue(connection.Input.Node, out var viewInput);
            Nodes.TryGetValue(connection.Output.Node, out var viewOutput);
            if (viewInput == null || viewOutput == null)
            {
                throw new Exception(
                    $"View node not found for input ({connection.Input.Node}) or output ({connection.Output.Node})");
            }
            
            var connView = new ReteConnection(connection, viewInput, viewOutput, Emitter);
            ConnectionViews.Add(connection, connView);
            Area.AppendChild(connView.HtmlElement);
        }

        public void RemoveConnection(Connection connection)
        {
            ConnectionViews.TryGetValue(connection, out var connView);
            ConnectionViews.Remove(connection);
            if (connView != null)
            {
                Area.RemoveChild(connView.HtmlElement);
            }
        }

        public void UpdateConnections(Node node, Point prevPoint)
        {
            var connections = node.GetConnections();
            foreach (var connection in connections)
            {
                ConnectionViews.TryGetValue(connection, out var connectionView);
                if (connectionView == null)
                {
                    Emitter.OnWarn("Connection view not found");
                    continue;
                }
                connectionView.Update();
            }
        }

        public void Resize()
        {
            
            /*
             TODO
             * const { container } = this;

        if (!container.parentElement)
            throw new Error('Container doesn\'t have parent element');

        const width = container.parentElement.clientWidth;
        const height = container.parentElement.clientHeight;

        container.style.width = width + 'px';
        container.style.height = height + 'px';
             */
        }

        public void Click(MouseEventArgs mouseEventArgs)
        {
            //ToDO
            //if (container !== e.target) return;
            Emitter.OnClick(mouseEventArgs, Container);
        }
    }
}