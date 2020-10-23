using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace retecs.ReteCs.View
{
    public class Drag
    {
        public (double X, double Y)? PointerStart { get; set; }
        public ElementReference El { get; set; }
        public Action<MouseEventArgs> OnStart { get; set; }
        public Action<MouseEventArgs> OnDrag { get; set; }
        public Action Destroy { get; set; }

        public Action<double, double, MouseEventArgs> OnTranslate { get; set; }

        public Drag(ElementReference container, Action<double, double, MouseEventArgs> onTranslate,
            Action<MouseEventArgs> onStart, Action<MouseEventArgs> onDrag = null)
        {
            PointerStart = null;
            OnStart = onStart;
            OnDrag = onDrag;
            OnTranslate = onTranslate;
            /*
             TODO
            El = container;
            El.style.touchAction = "none";
            El.addEventListener("pointerdown", this.down.bind(this));*/

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
            PointerStart = (mouseEventArgs.ClientX, mouseEventArgs.ClientY);
            OnStart(mouseEventArgs);
        }

        public void Move(MouseEventArgs mouseEventArgs)
        {
            if (!PointerStart.HasValue)
            {
                return;
            }

            var x = mouseEventArgs.ClientX;
            var y = mouseEventArgs.ClientY;

            var deltaX = x - PointerStart.Value.Item1;
            var deltaY = y - PointerStart.Value.Item2;

            // TODO get getBoundingClientRect via Javascript
            var zoom = 1;
            //var zoom = this.el.getBoundingClientRect().width / this.el.offsetWidth;
            OnTranslate(deltaX / zoom, deltaY / zoom, mouseEventArgs);
        }

        public void Up(MouseEventArgs mouseEventArgs)
        {
            if (!PointerStart.HasValue)
            {
                return;
            }

            PointerStart = null;
            OnDrag(mouseEventArgs);
        }
    }
}