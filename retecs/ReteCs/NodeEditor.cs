using System;
using System.Collections.Generic;
using System.Text.Json;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;
using retecs.Shared;

namespace retecs.ReteCs
{
    public class NodeEditor: Context
    {
        public List<Node> Nodes { get; set; } = new List<Node>();
        public Selected Selected { get; set; } = new Selected();
        public ReteEditor View { get; set; }
        public bool Silent { get; set; }

        public NodeEditor(string id, Emitter emitter) : base(id, emitter)
        {
            View = new ReteEditor(Components, emitter);
            Nodes = new List<Node>();

            Emitter.WindowKeyDown += Emitter.OnKeyDown;
            Emitter.WindowKeyUp += Emitter.OnKeyUp;
            Emitter.Destroy += () =>
            {
                Emitter.WindowKeyDown -= Emitter.OnKeyDown;
                Emitter.WindowKeyUp -= Emitter.OnKeyUp;
            };

            Emitter.SelectNode += OnSelectNodeEventHandler;
            Emitter.NodeSelected += OnNodeSelectedEventHandler;
            Emitter.TranslateNode += OnTranslateNodeEventHandler;
        }

        public void AddNode(Node node)
        {
            Emitter.OnNodeCreate(node);
            Nodes.Add(node);
            View.AddNode(node);
            Emitter.OnNodeCreated(node);
        }

        public void RemoveNode(Node node)
        {
            Emitter.OnNodeRemove(node);
            node.GetConnections().ForEach(c => RemoveConnection(c));
            Nodes.Remove(node);
            View.RemoveNode(node);
            Emitter.OnNodeRemoved(node);
        }

        public void Connect(Output output, Input input, object data = null)
        {
            Emitter.OnConnectionCreate(input, output);
            try
            {
                var con = output.ConnectTo(input);
                con.Data = data;
                View.AddConnection(con);
                Emitter.OnConnectionCreated(con);
            }
            catch (Exception e)
            {
                Emitter.OnWarn("Exception: "+ e.Message, e.InnerException?.Message);
            }
        }

        public void RemoveConnection(Connection connection)
        {
            Emitter.OnConnectionRemove(connection);
            View.RemoveConnection(connection);
            connection.Remove();
            Emitter.OnConnectionRemoved(connection);
        }

        private void OnSelectNodeEventHandler(Node node, bool accumulate)
        {
            if (!Nodes.Contains(node))
            {
                Emitter.OnError($"Node not exist in list {node?.Name}");
                return;
            }
            Emitter.OnNodeSelect(node);
            Selected.Add(node, accumulate);
            Emitter.OnNodeSelected(node);
        }

        public Component GetComponent(string name)
        {
            Components.TryGetValue(name, out var component);
            return component;
        }

        public new void Register(Component component)
        {
            base.Register(component);
            component.Editor = this;
        }

        public void Clear()
        {
            Nodes.ForEach(n => RemoveNode(n));
            Emitter.OnClear();
        }

        public Data ToJson()
        {
            var data = new Data
            {
                Id = Id,
                Nodes = new Dictionary<string, NodeData>()
            };
            Nodes.ForEach(n => data.Nodes[n.Id] = n.ToJson());
            Emitter.OnExport(data);
            return data;
        }

        public bool BeforeImport(Data data)
        {
            var (success, message) = Validator.Validate(Id, data);
            if (!success)
            {
                Emitter.OnError(message);
                return false;
            }

            Silent = true;
            Clear();
            Emitter.OnImport(data);
            return true;
        }

        public bool AfterImport()
        {
            Silent = false;
            return true;
        }

        public bool FromData(Data data)
        {
            if (!BeforeImport(data))
            {
                return false;
            }

            var nodes = new Dictionary<string, Node>();

            try
            {
                foreach (var node in data.Nodes.Values)
                {
                    // Create node
                    var component = GetComponent(node.Name);
                    nodes[node.Id] = component.Build(Node.FromJson(node));
                    AddNode(nodes[node.Id]);
                }
                foreach (var node in data.Nodes.Values)
                {
                    foreach (var (key, _) in node.Outputs)
                    {
                        foreach (var con in node.Outputs[key].Connections)
                        {
                            var nodeId = con.Node;
                            var restoreData = con.Data;
                            var targetOutput = nodes[nodeId].Outputs[key];
                            var targetInput = nodes[nodeId].Inputs[con.Input];
                            if (targetInput == null || targetOutput == null)
                            {
                                Emitter.OnError($"IO not found for node {node.Id}");
                                continue;
                            }
                            Connect(targetOutput, targetInput, restoreData);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Emitter.OnWarn(e.Message, e);
                return !AfterImport();
            }

            return AfterImport();
        }

        private void OnTranslateNodeEventHandler(Node node, Point point)
        {
            Selected.Each(n =>
            {
                View.Nodes.TryGetValue(n, out var nodeView);
                if (nodeView == null)
                {
                    Emitter.OnError("Could not find NodeView for Node: " + JsonSerializer.Serialize(node));
                    return;
                }
                nodeView.OnDrag(point);
            });
        }

        private void OnNodeSelectedEventHandler(Node node)
        {
            Selected.Each(n =>
            {
                View.Nodes.TryGetValue(n, out var nodeView);
                nodeView?.OnStart();
            });
        }
    }
}
