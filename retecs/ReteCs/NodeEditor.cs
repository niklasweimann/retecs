using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using retecs.ReteCs.core;
using retecs.ReteCs.Interfaces;

namespace retecs.ReteCs
{
    public class NodeEditor: Context<EventsTypes>
    {
        public List<Node> Nodes { get; set; }
        public Selected Selected { get; set; }
        public EditorView View { get; set; }
        public NodeEditor(string id, ElementReference container) : base(id, new EditorEvents())
        {
            View = new EditorView(container, Components, this);
            
            On(new List<string>{"destroy"}, )}
        }
    }
}