using retecs.RenderPlugin.Entities;
using retecs.ReteCs.core;

namespace retecs.Components
{
    public class NumControl : BlazorControl
    {
        public NumControl(string key, Emitter emitter) : base(key)
        {
            Emitter = emitter;
            Type = "number";
        }

        public void OnChange(int number)
        {
            SetValue(number);
            Emitter.OnProcess();
        }

        public void SetValue(int number)
        {
            Value = number;
            PutData(Key, number);
        }
    }
}