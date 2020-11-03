using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using retecs.ReteCs;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;

namespace retecs.Shared
{
    public partial class ReteNode
    {
        [Inject]
        public Emitter Emitter { get; set; }

        [Parameter]
        public Node Node { get; set; }
        [Parameter]
        public NodeEditor Editor { get; set; }

        public Point PointerStart { get; set; }
        public Component Component { get; set; }
        public Point StartPosition { get; set; }

        public List<string> Styles { get; set; } = new List<string>();


        public ReteNode()
        {
            
        }

        public ReteNode(Node node, Component component, Emitter emitter)
        {
            Node = node;
            Emitter = emitter;
            Component = component;
            // todo this.el.addEventListener('contextmenu', e => this.trigger('contextmenu', { e, node: this.node }));
            Emitter.OnRenderNode(node, component.Data, (reference, type, io) => { },
                (reference, control) => { });

            UpdateStyle();
        }

        public void OnSelect(MouseEventArgs mouseEventArgs)
        {
            OnStart();
            Emitter.OnMultiSelectNode(Node, mouseEventArgs.ShiftKey, mouseEventArgs);
            Emitter.OnSelectNode(Node, mouseEventArgs.ShiftKey);
        }

        public void OnStart()
        {
            StartPosition = Node.Position;
        }

        public void OnTranslate(Point point)
        {
            Emitter.OnTranslateNode(Node, point);
        }

        public void OnDrag(Point point)
        {
            var x = StartPosition.X + point.X;
            var y = StartPosition.Y + point.Y;
            Translate(x, y);
        }

        public void Translate(double x, double y)
        {
            Emitter.OnNodeTranslate(Node, x, y);
            var prev = Node.Position;
            Node.Position = new Point(x, y);
            UpdateStyle();
            Emitter.OnNodeTranslated(Node, prev);
        }

        public void UpdateStyle()
        {
            Styles = new List<string>
            {
                "position: absolute",
                "touch-action: none",
            };
            if (Node.Position != null)
            {
                Styles.Add($"transform: translate({Node.Position.X}px, {Node.Position.Y}px)");
            }
        }

        public string GetStyle()
        {
            UpdateStyle();
            return string.Join(";", Styles);
        }

        private string NodeClasses()
        {
            var classes = new List<string>();
            if (Node != null)
            {
                if (Editor != null && Editor.Selected.Contains(Node))
                {
                    classes.Add("selected");
                }
                classes.Add(Node.Name.ToLower());
            }
            classes.Add("node");
            return string.Join(" ", classes);
        }

        public void Down(MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Type == "mouse" && mouseEventArgs.Button != 0)
            {
                return;
            }

            PointerStart = new Point(mouseEventArgs.ClientX, mouseEventArgs.ClientY);
            OnSelect(mouseEventArgs);
        }

        public void Move(MouseEventArgs mouseEventArgs)
        {
            if (PointerStart == null)
            {
                return;
            }


            var x = mouseEventArgs.ClientX;
            var y = mouseEventArgs.ClientY;

            var deltaX = x - PointerStart.X;
            var deltaY = y - PointerStart.Y;

            // TODO get getBoundingClientRect via Javascript
            //var zoom = this.el.getBoundingClientRect().width / this.el.offsetWidth;
            var zoom = 1;
            OnTranslate(new Point(deltaX / zoom, deltaY / zoom));
        }

        public void Up(MouseEventArgs eventArgs)
        {
            if (PointerStart == null)
            {
                return;
            }

            PointerStart = null;
            Emitter.OnNodeDragged(Node);
        }

        public void Leave(MouseEventArgs leave)
        {
            PointerStart = null;
        }
    }
}