using System.Collections.Generic;

namespace retecs.ReteCs.core
{
    public class Plugin
    {
        public string Name { get; set; }
        public (Dictionary<string, object> context, Dictionary<string, object> options) Install { get; set; }
    }
}