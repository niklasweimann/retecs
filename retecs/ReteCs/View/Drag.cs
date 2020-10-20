using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace retecs.ReteCs.View
{
    public class Drag
    {
        public (int, int) PointerStart { get; set; }
        public Action Destroy;
        public HtmlElement El { get; set; }

        public Drag(ElementReference container, Action<int, int> onTranslate, Action onStart, Action onDrag)
        {
            PointerStart = null;
            El = container;
            El.style.touchAction = "none";
            El.addEventListener("pointerdown", this.down.bind(this));

            var destroyMove = listenWindow('pointermove', this.move.bind(this));
            var destroyUp = listenWindow('pointerup', this.up.bind(this));

            this.destroy = () => { destroyMove(); destroyUp(); }
        }

        public void Down(MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Type == "mouse" && mouseEventArgs.Button != 0)
            {
                return;
            }
        }
    }
}