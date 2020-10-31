﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using retecs.ReteCs;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;

namespace retecs.Components
{
    public class NumOutComponent : Component 
    {
        public Emitter Emitter { get; }

        public NumOutComponent(Emitter emitter) : base("Number Output")
        {
            Emitter = emitter;
        }

        public override void Worker(NodeData node, Dictionary<string, List<object>> inputs, Dictionary<string, object> outputs, params object[] args)
        {
            Emitter.OnInfo("NodeInput: ", inputs);
            var input = inputs["innum"] ?? new List<object>();
            var editorNode = Editor.Nodes.FirstOrDefault(x => x.Id == node.Id);
            if (editorNode == null)
            {
                Emitter.OnWarn("Node could not be found. Id was: " + node.Id);
            }
            else
            {
                const string controlKey = "num";
                var ctrl = editorNode.Controls[controlKey];
                if (ctrl == null)
                {
                    Emitter.OnWarn($"Control could not be found. Key was: {controlKey}. But Node only has this keys:",
                        Editor.Nodes.FirstOrDefault(x => x.Id == node.Id)?.Controls.Values.Select(x => x.Key));
                }
                ((NumControl)ctrl)?.SetValue(input.FirstOrDefault());
            }
            
        }

        public override void Builder(Node node)
        {
            var in1 = new Input("innum", "Number", Sockets.NumberSocket);
            node.AddControl(new NumControl(Emitter, "num")
            {
                Readonly = true
            }).AddInput(in1);
        }
    }
}