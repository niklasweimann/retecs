﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;

namespace retecs.ReteCs.View
{
    public class EditorView
    {
        public Emitter Emitter { get; set; }
        public ElementReference Container { get; set; }
        public Dictionary<string, Component> Components { get; set; }
        public Dictionary<Node, NodeView> Nodes { get; } = new Dictionary<Node, NodeView>();
        public Dictionary<Connection, ConnectionView> Connections { get; } = new Dictionary<Connection, ConnectionView>();
        public Area Area { get; set; }

        public EditorView(ElementReference container, Dictionary<string, Component> components, Emitter emitter)
        {
            Emitter = emitter;
            Container = container;
            Components = components;
            // TODO
            //this.container.style.overflow = 'hidden';
            Emitter.Click += (args, reference) => Click(args);
            Emitter.WindowContextMenu += args =>  Emitter.OnContextMenu(args);
            Emitter.WindowResize += Resize;
            Emitter.Destroy += () =>
            {
                Emitter.WindowResize -= Resize;
                foreach (var nodesValue in Nodes.Values)
                {
                    nodesValue.Destroy();
                }
            };

            Emitter.NodeTranslated += UpdateConnections;
            Area = new Area(container, Emitter);
            // Destroyed
        }

        public void AddNode(Node node)
        {
            Components.TryGetValue(node.Name, out var component);
            if (component == null)
            {
                throw new Exception($"Component {node.Name} not found");
            }
            var nodeView = new NodeView(node, component, Emitter);
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
                nodeView.Destroy();
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
                throw new Exception($"View node not found for input ({viewInput}) or output ({viewOutput})");
            }
            
            var connView = new ConnectionView(connection, viewInput, viewOutput, Emitter);
            Connections.Add(connection, connView);
            Area.AppendChild(connView.HtmlElement);
        }

        public void RemoveConnection(Connection connection)
        {
            Connections.TryGetValue(connection, out var connView);
            Connections.Remove(connection);
            if (connView != null)
            {
                Area.RemoveChild(connView.HtmlElement);
            }
        }

        public void UpdateConnections(Node node, Point prevPoint)
        {
            foreach (var connection in node.GetConnections())
            {
                Connections.TryGetValue(connection, out var connectionView);
                if (connectionView == null)
                {
                    throw new Exception("Connection view not found");
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