using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using Microsoft.AspNetCore.Components;
using retecs.BlazorServices;
using retecs.Components;
using retecs.RenderPlugin;
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
        private int _seq;

        public int Sequence => ++_seq;

        //Unused since blazor needs this internally
        public ReteEditor()
        {
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            Editor.Emitter.RenderConnection += (connection, input, output, inputElementReference, outputElementReference) =>
                                               {
                                                   _renderFragment += builder =>
                                                                      {
                                                                          builder.OpenComponent<ReteConnection>(Sequence);
                                                                          builder.AddAttribute(Sequence, nameof(ReteConnection.Connection),
                                                                              connection);
                                                                          builder.AddAttribute(Sequence, nameof(ReteConnection.Input),
                                                                              input);
                                                                          builder.AddAttribute(Sequence, nameof(ReteConnection.Output),
                                                                              output);
                                                                          builder.AddAttribute(Sequence,
                                                                              nameof(ReteConnection.InputElementReference),
                                                                              inputElementReference);
                                                                          builder.AddAttribute(Sequence,
                                                                              nameof(ReteConnection.OutputElementReference),
                                                                              outputElementReference);
                                                                          builder.CloseComponent();
                                                                      };
                                                   StateHasChanged();
                                               };
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            static string Serialize(object data)
            {
                if (data != null)
                {
                    return "; Data: " + JsonSerializer.Serialize(data);
                }

                return "";
            }

            Editor = new NodeEditor("rete@0.0.1", Emitter);
            Editor.Emitter.WindowKeyDown += _ => Console.WriteLine("WindowKeyDown");
            Editor.Emitter.WindowKeyUp += _ => Console.WriteLine("WindowKeyUp");
            Editor.Emitter.WindowMouseMove += _ => Console.WriteLine("WindowMouseMove");
            Editor.Emitter.WindowMouseUp += _ => Console.WriteLine("WindowMouseUp");
            Editor.Emitter.Warn += (warning, data) => Console.WriteLine("Warning: " + warning + Serialize(data));
            Editor.Emitter.Error += (error, data) => Console.WriteLine("Error: " + error + Serialize(data));
            Editor.Emitter.Info += (info, data) => Console.WriteLine("Info: " + info + Serialize(data));
            Editor.Emitter.Debug += (info, data) => Console.WriteLine("Debug: " + info + Serialize(data));

            Editor.Use(new BasicRenderer());
            // editor.use(ContextMenuPlugin);

            Engine = new Engine("rete@0.0.1", Emitter);
            var components = new List<Component> {new NumComponent(Emitter), new NumOutComponent(Emitter)};
            components.ForEach(component =>
                               {
                                   Editor.Register(component);
                                   Engine.Register(component);
                               });

            var n1 = components[0]
                .CreateNode(new Dictionary<string, object>
                            {
                                {"num", 2}
                            });
            var n2 = components[1]
                .CreateNode(new Dictionary<string, object>());
            var n3 = components[1]
                .CreateNode(new Dictionary<string, object>());
            n1.Position = new Point(80, 200);
            n2.Position = new Point(400, 200);
            n3.Position = new Point(400, 400);
            Editor.Emitter.Process += RequestAnimationFrame;
            Editor.Emitter.ConnectionCreated += _ => RequestAnimationFrame();
            Editor.Emitter.ConnectionRemoved += _ => RequestAnimationFrame();
            Editor.Emitter.NodeRemoved += _ => RequestAnimationFrame();
            Editor.Emitter.NodeCreated += _ => RequestAnimationFrame();

            Editor.Emitter.NodeCreated += node =>
                                          {
                                              _renderFragment += builder =>
                                                                 {
                                                                     builder.OpenComponent<ReteNode>(Sequence);
                                                                     builder.AddAttribute(Sequence, "Editor", Editor);
                                                                     builder.AddAttribute(Sequence, "Node", node);
                                                                     builder.CloseComponent();
                                                                 };
                                          };
            Editor.AddNode(n1);
            Editor.AddNode(n2);
            Editor.AddNode(n3);
            //Editor.Connect(n1.Outputs["num"], n2.Inputs["innum"]);
            Engine.Emitter.OnProcess();
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
