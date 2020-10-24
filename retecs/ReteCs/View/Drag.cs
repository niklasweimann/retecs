using System;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using retecs.ReteCs.Entities;
using retecs.ReteCs.JsInterop;

namespace retecs.ReteCs.View
{
    public class Drag
    {
        public Point PointerStart { get; set; }
        public ElementReference HtmlReference { get; }
        public Action<MouseEventArgs> OnStart { get; }
        public Action<MouseEventArgs> OnDrag { get; }
        public Action Destroy { get; }
        public EventInterop EventInterop { get; } = new EventInterop();

        public Action<Point, MouseEventArgs> OnTranslate { get; }

        public Drag(ElementReference container, Action<Point, MouseEventArgs> onTranslate,
            Action<MouseEventArgs> onStart, Action<MouseEventArgs> onDrag = null)
        {
            PointerStart = null;
            OnStart = onStart;
            OnDrag = onDrag;
            OnTranslate = onTranslate;
            HtmlReference = container;
            /*
             TODO
            El.style.touchAction = "none";*/
            EventInterop.AddEventListener(HtmlReference, "pointerdown", mouse =>
            {
                Console.WriteLine(JsonSerializer.Serialize((MouseEventArgs)mouse));
                Down((MouseEventArgs)mouse);
            });

            var destroyMove = Utils.ListenWindow("pointermove", Move);
            var destroyUp = Utils.ListenWindow("pointerup", Up);

            Destroy = () =>
            {
                destroyMove();
                destroyUp();
            };
        }

        public void Down(MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Type == "mouse" && mouseEventArgs.Button != 0)
            {
                return;
            }

            //TODO stop propagation
            PointerStart = new Point(mouseEventArgs.ClientX, mouseEventArgs.ClientY);
            OnStart(mouseEventArgs);
        }

        public void Move(object eventArgs)
        {
            var mouseEventArgs = (MouseEventArgs) eventArgs;
            if (PointerStart == null)
            {
                return;
            }

            var x = mouseEventArgs.ClientX;
            var y = mouseEventArgs.ClientY;

            var deltaX = x - PointerStart.X;
            var deltaY = y - PointerStart.Y;

            // TODO get getBoundingClientRect via Javascript
            var zoom = 1;
            //var zoom = this.el.getBoundingClientRect().width / this.el.offsetWidth;
            OnTranslate(new Point(deltaX / zoom, deltaY / zoom), mouseEventArgs);
        }

        public void Up(object eventArgs)
        {
            var mouseEventArgs = (MouseEventArgs) eventArgs;
            if (PointerStart == null)
            {
                return;
            }

            PointerStart = null;
            OnDrag(mouseEventArgs);
        }
    }
}