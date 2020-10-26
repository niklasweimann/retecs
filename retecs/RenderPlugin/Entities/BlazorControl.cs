using System;
using retecs.ReteCs;
using retecs.ReteCs.core;

namespace retecs.RenderPlugin.Entities
{
    public class BlazorControl: Control
    {
        public string Render { get; } = "basicBlazorRenderer";
        public Emitter Emitter { get; set; }
        public bool Readonly { get; set; }
        public Action Change { get; set; }
        public int Value { get; set; }
        public Action Mounted { get; set; }
        public string Type { get; set; }

        public BlazorControl(string key) : base(key)
        {
        }
    }
}