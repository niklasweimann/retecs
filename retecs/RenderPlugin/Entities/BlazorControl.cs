using System;
using retecs.ReteCs;
using retecs.ReteCs.core;

namespace retecs.RenderPlugin.Entities
{
    public class BlazorControl: Control
    {
        public bool Readonly { get; set; }
        public Action<object> ChangeHandler { get; set; }
        public object Value { get; set; }
        public Action Mounted { get; set; }
        public string Type { get; set; }

        public BlazorControl(string key, Emitter emitter) : base(key, emitter)
        {
        }
    }
}