using retecs.ReteCs;
using retecs.ReteCs.core;

namespace retecs.Shared
{
    public partial class ReteControl
    {
        private Emitter Emitter { get; }

        public ReteControl()
        {
            
        }

        public ReteControl(Control control, Emitter emitter)
        {
            Emitter = emitter;
            Emitter.OnRenderControl(control);
        }
    }
}