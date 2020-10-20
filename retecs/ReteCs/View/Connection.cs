using System;
using System.Collections.Generic;
using retecs.ReteCs.core;
using retecs.ReteCs.Interfaces;

namespace retecs.ReteCs.View
{
    public class Connection: Emitter<EventsTypes>
    {
        public Connection(Dictionary<string, List<Func<object, bool>>> events) : base(events)
        {
            
        }
    }
}