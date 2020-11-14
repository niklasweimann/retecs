using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Components;
using retecs.BlazorServices;
using retecs.ReteCs;
using retecs.ReteCs.core;
using retecs.ReteCs.Engine;
using retecs.ReteCs.Entities;
using Component = retecs.ReteCs.Component;

namespace retecs.Shared
{
    public partial class ReteEditor
    {
        [Inject]
        private BrowserService BrowserService { get; set; }

        private Emitter Emitter { get; } = SingletonEmitter.Instance;

        public Dictionary<string, Component> Components { get; }

        public Dictionary<Node, ReteNode> Nodes { get; } = new Dictionary<Node, ReteNode>();

        public Dictionary<Connection, ReteConnection> ConnectionViews { get; } = new Dictionary<Connection, ReteConnection>();

        public ReteArea Area { get; }

        public NodeEditor Editor { get; set; }

        public Engine Engine { get; set; }

        private RenderFragment _renderFragment;

        //Unused since blazor needs this internally
        public ReteEditor()
        {
        }

        public ReteEditor(Dictionary<string, Component> components, Emitter emitter)
        {
            Emitter = emitter;
            Components = components;
            Emitter.WindowContextMenu += args => Emitter.OnContextMenu(args);
            Emitter.NodeTranslated += UpdateConnections;
            Area = new ReteArea(Emitter);
        }

        private void RequestAnimationFrame()
        {
            Engine.Abort();
            Engine.ProcessData(Editor.ToJson(), "0");
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
        }

        public void RemoveNode(Node node) => Nodes.Remove(node);

        public void AddConnection(Connection connection)
        {
            if (connection.Input.Node == null ||
                connection.Output.Node == null)
            {
                throw new Exception("Connection input or output not added to node");
            }

            Nodes.TryGetValue(connection.Input.Node, out var viewInput);
            Nodes.TryGetValue(connection.Output.Node, out var viewOutput);
            if (viewInput == null ||
                viewOutput == null)
            {
                throw new Exception($"View node not found for input ({connection.Input.Node}) or output ({connection.Output.Node})");
            }

            //var connView = new ReteConnection(connection, viewInput, viewOutput, Emitter);
            // todo: ConnectionViews.Add(connection, connView);
        }

        public void RemoveConnection(Connection connection) => ConnectionViews.Remove(connection);

        public void UpdateConnections(Node node, Point prevPoint)
        {
            //var connections = node.GetConnections();
            //foreach (var connection in connections)
            //{
            //   Registered resize listener to window ConnectionViews.TryGetValue(connection, out var connectionView);
            //    if (connectionView == null)
            //    {
            //        Emitter.OnWarn("Connection view not found");
            //        continue;
            //    }
            //    connectionView.Update();
            //}
        }
    }
}
