using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using retecs.ReteCs.core;

namespace retecs.ReteCs.View
{
    public class ControlView : Emitter
    {
        public ControlView(ElementReference el, ReteCs.Control control)
        {
            OnRenderControl(el, control);
        }
    }
}