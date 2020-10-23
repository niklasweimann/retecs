using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using retecs.ReteCs.core;
using retecs.ReteCs.View;

namespace retecs.ReteCs
{
    public class NodeEditor: Context
    {
        public List<Node> Nodes { get; set; }
        public Selected Selected { get; set; }
        public EditorView View { get; set; }
        public NodeEditor(string id, ElementReference container) : base(id)
        {
            View = new EditorView(container, Components);

            OnDestroy();
        }
    }
}