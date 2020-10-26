using Microsoft.AspNetCore.Components;
using retecs.ReteCs.core;

namespace retecs.ReteCs.View
{
    public class ControlView
    {
        public Emitter Emitter { get; set; }
        public ControlView(ElementReference el, Control control, Emitter emitter)
        {
            Emitter = emitter;
            Emitter.OnRenderControl(el, control);
        }
    }
}