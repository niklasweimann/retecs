using System;
using System.Collections.Generic;

namespace retecs.ReteCs.core
{
    public class Emitter<T>
    {
        public Dictionary<string, Func<object>> Events { get; set; }
        public bool Silent { get; set; }

        public Emitter(Events events)
        {
            
        }

        public Emitter<T> On<>()
        {
            
        }
    }
}