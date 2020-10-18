using System;
using System.Collections.Generic;

namespace retecs.ReteCs.core
{
    public class Plugin
    {
        public string Name { get; set; }
        public Action<object, object> Install { get; set; }
    }
}