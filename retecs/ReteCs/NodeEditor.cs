﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;
using retecs.ReteCs.View;

namespace retecs.ReteCs
{
    public class NodeEditor: Context
    {
        public List<Node> Nodes { get; set; }
        public Selected Selected { get; set; } = new Selected();
        public EditorView View { get; set; }
        public bool Silent { get; set; }
        
        public NodeEditor(string id, ElementReference container, Emitter emitter) : base(id, emitter)
        {
            View = new EditorView(container, Components, emitter);
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

        public void Connect(Output output, Input input, object data)
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
                Emitter.OnWarn(e.Message, e);
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
                throw new Exception($"Node not exist in list {node.Name}");
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

        public void Register(Component component)
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
            var data = new Data();
            Nodes.ForEach(n => data.Nodes[n.Id] = n.ToJson());
            Emitter.OnExport(data);
            return data;
        }

        public bool BeforeImport(Data data)
        {
            var (success, message) = Validator.Validate(Id, data);
            if (!success)
            {
                Emitter.OnWarn(message);
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

            Dictionary<string, Node> nodes = new Dictionary<string, Node>();
            
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
                    foreach (var output in node.Outputs)
                    {
                        foreach (var con in node.Outputs[output.Key].Connections)
                        {
                            var nodeId = con.Node;
                            var restoreData = con.Data;
                            var targetOutput = nodes[nodeId].Outputs[output.Key];
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
                nodeView?.OnDrag(point);
            });
        }

        private void OnNodeSelectedEventHandler(Node node)
        {
            void Action(Node n)
            {
                View.Nodes.TryGetValue(n, out var nodeView);
                nodeView?.OnStart();
            }

            Selected.Each(Action);
        }
    }
}