using System;
using System.Collections.Generic;

namespace retecs.ReteCs.core
{
    public class Events
    {
        public Handler Handlers { get; set; }

        public Events(Handler handlers)
        {
            Handlers = handlers;
        }
    }

    public class Handler
    {
        public Action Warn { get; set; } = Console.WriteLine;
        public Action Error { get; set; } = Console.Error.WriteLine;
        public List<object> ComponentRegister { get; set; }
        public List<object> Destroy { get; set; }
    }
}