using System;
using retecs.RenderPlugin.Entities;
using retecs.ReteCs.core;

namespace retecs.Components
{
    public class NumControl : BlazorControl
    {
        public NumControl(Emitter emitter, string key, bool @readonly = false) : base(key)
        {
            Emitter = emitter;
            Type = "number";
            Change = OnChange;
            Value = 0;
            Mounted = () =>
            {
                SetValue(GetData(key));
            };
        }

        public void OnChange(object number)
        {
            if (!string.Equals("number", Type) || number == null)
            {
                return;
            }

            if(number is int numberValue)
            {
                SetValue(numberValue);
                Emitter.OnProcess();
            }
        }

        public void SetValue(object number)
        {
            if (number is int numberValue)
            {
                Value = numberValue;
                PutData(Key, numberValue);
                return;
            }

            Emitter.OnError("Input of set value is of the wrong type. Should be int, but was: " + number?.GetType());
        }
    }
}