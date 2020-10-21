using System;
using System.Collections.Generic;
using retecs.ReteCs.core;
using retecs.ReteCs.Interfaces;

namespace retecs.ReteCs.View
{
    public class Control : Emitter<EventsTypes>
    {
        public Control(HtmlElement el, ReteCs.Control control,
            Dictionary<string, List<Func<object, bool>>> events) : base(events)
        {
            Trigger("rendercontrol", new {el, control});
        }
    }
}