using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;

namespace retecs.ReteCs.View
{
    public class NodeView : Emitter
    {
        public Node Node { get; set; }
        public Component Component { get; set; }
        public Dictionary<Io, SocketView> Sockets { get; set; }
        public Dictionary<Control, ControlView> Controls { get; set; }

        public ElementReference HtmlElement { get; set; }
        public Point StartPosition { get; set; }
        public Drag Drag { get; set; }

        public NodeView(Node node, Component component)
        {
            Node = node;
            Component = component;
            //htmlElement = new ElementReference();
            /*
             * this.el = document.createElement('div');
        this.el.style.position = 'absolute';
             */
            //this.el.addEventListener('contextmenu', e => this.trigger('contextmenu', { e, node: this.node }));
            Drag = new Drag(HtmlElement, (point, _) => OnTranslate(point), mouseEventArgs => OnSelect(mouseEventArgs), (_) =>
            {
                OnNodeDragged(node);
            });
            OnRenderNode(HtmlElement, node, component.Data, (reference, type, io) => BindSocket(reference, type, io),
                (reference, control) => BindControl(reference, control));
            Update();
        }

        public void ClearSocket()
        {
            var ios = new List<Io>();
            ios.AddRange(Node.Inputs.Values);
            ios.AddRange(Node.Outputs.Values);

            foreach (var keyValuePair in Sockets.Where(keyValuePair => !ios.Contains(keyValuePair.Key)))
            {
                Sockets.Remove(keyValuePair.Key);
            }
        }

        public void BindSocket(ElementReference htmlElement, string type, Io io)
        {
            ClearSocket();
            Sockets.Add(io, new SocketView(htmlElement, type, io, Node));
        }

        public void BindControl(ElementReference htmlElement, Control control)
        {
            Controls.Add(control, new ControlView(htmlElement, control));
        }

        public Point GetSocketPosition(Io io)
        {
            Sockets.TryGetValue(io, out var value);
            if (value == null)
            {
                throw new Exception($"Socket not found for ${io.Name} with key ${io.Key}");
            }

            return value.GetPosition(Node.Position);
        }

        public void OnSelect(MouseEventArgs mouseEventArgs)
        {
            OnStart();
            OnMultiSelectNode(Node, mouseEventArgs.ShiftKey, mouseEventArgs);
            OnSelectNode(Node, mouseEventArgs.ShiftKey);
        }

        public void OnStart()
        {
            StartPosition = Node.Position;
        }

        public void OnTranslate(Point point)
        {
            OnTranslateNode(Node, point);
        }

        public void OnDrag(Point point)
        {
            var x = StartPosition.X + point.X;
            var y = StartPosition.Y + point.Y;
            Translate(x, y);
        }

        public void Translate(double x, double y)
        {
            var param = new {Node, x, y};
            OnNodeTranslate(Node, x, y);
            var prev = Node.Position;
            Node.Position = new Point(param.x, param.y);
            Update();
            OnNodeTranslated(Node, prev);
        }

        public void Update()
        {
            var pos = Node.Position;
            
            //TODO
            //this.el.style.transform = `translate(${x}px, ${y}px)`;
        }

        public void Remove()
        {
            
        }

        public void Destroy()
        {
            Drag.Destroy();
        }
    }
}