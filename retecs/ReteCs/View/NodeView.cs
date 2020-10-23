using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using retecs.ReteCs.core;

namespace retecs.ReteCs.View
{
    public class NodeView : Emitter
    {
        public Node Node { get; set; }
        public Component Component { get; set; }
        public Dictionary<Io, SocketView> Sockets { get; set; }
        public Dictionary<Control, ControlView> Controls { get; set; }

        public ElementReference HtmlElement { get; set; }
        public (double x, double y) StartPosition { get; set; }
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
            Drag = new Drag(HtmlElement, (double, double, MouseEventArgs) => OnTranslate(), () => OnSelect(), () =>
            {
                OnNodeDragged(node);
            });
            OnRenderNode(HtmlElement, node, component.Data, () => BindSocket(), () => BindControl());
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

        public (double posX, double posY) GetSocketPosition(Io io)
        {
            Sockets.TryGetValue(io, out var value);
            if (value == null)
            {
                throw new Exception($"Socket not found for ${io.Name} with key ${io.Key}");
            }

            return value.GetPosition(((double)Node.Position.Item1, (double)Node.Position.Item2));
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

        public void OnTranslate(int dx, int dy)
        {
            OnTranslateNode(Node, dx, dy);
        }

        public void OnDrag(int dx, int dy)
        {
            var x = StartPosition.x + dx;
            var y = StartPosition.y + dy;
            Translate(x, y);
        }

        public void Translate(double x, double y)
        {
            var param = new {Node, x, y};
            if (Trigger("nodetranslate", param))
            {
                return;
            }

            var prev = Node.Position;
            Node.Position = (param.x, param.y);
            Update();
            OnNodeTranslated(Node, prev.X, prev.Y);
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