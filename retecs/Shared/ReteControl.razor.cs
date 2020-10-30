using Microsoft.AspNetCore.Components;
using retecs.ReteCs;
using retecs.ReteCs.core;

namespace retecs.Shared
{
    public partial class ReteControl
    {
        public Emitter Emitter { get; set; }

        public ReteControl()
        {
            
        }

        public ReteControl(ElementReference el, Control control, Emitter emitter)
        {
            Emitter = emitter;
            Emitter.OnRenderControl(el, control);
        }
    }
}