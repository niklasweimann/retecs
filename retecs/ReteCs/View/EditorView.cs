using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using retecs.ReteCs.core;

namespace retecs.ReteCs.View
{
    public class EditorView : Emitter
    {
        public ElementReference Container { get; set; }
        public Dictionary<string, Component> Components { get; set; }
        public Dictionary<Node, NodeView> Nodes { get; set; }
        public Dictionary<Connection, ConnectionView> Connections { get; set; }
        public Area Area { get; set; }

        public EditorView(ElementReference container, Dictionary<string, Component> components)
        {
            Container = container;
            Components = components;
            // TODO
            //this.container.style.overflow = 'hidden';
            //this.container.addEventListener('click', this.click.bind(this));
            //this.container.addEventListener('contextmenu', e => this.trigger('contextmenu', { e, view: this }));

            //TODO weiter
        }
    }
}