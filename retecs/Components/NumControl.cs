using System;
using System.Text.Json;
using retecs.RenderPlugin.Entities;
using retecs.ReteCs.core;

namespace retecs.Components
{
    public class NumControl : BlazorControl
    {
        public NumControl(Emitter emitter, string key, bool @readonly = false) : base(key, emitter)
        {
            Type = "number";
            ChangeHandler = OnChange;
            Value = 0;
            Readonly = @readonly;
            Mounted = () =>
            {
                SetValue(GetData(key));
            };
        }

        public void OnChange(object number)
        {
            if (long.TryParse((string)number, out var numberValue))
            {
                Emitter.OnInfo("Changing Value to: " + numberValue);
                SetValue(numberValue);
                Emitter.OnProcess();
            }
            else
            {
                Emitter.OnWarn(nameof(OnChange) + " was called with a value that can not be converted to int.");
                Emitter.OnWarn("Value was: " + JsonSerializer.Serialize(new {number}));
            }
        }

        public void SetValue(object number)
        {
            Emitter.OnInfo("Write: " + number + "; To: " + Key);
            Value = number;
            PutData(Key, number);
        }
    }
}