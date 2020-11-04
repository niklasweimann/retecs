using System.Text.Json;
using retecs.RenderPlugin.Entities;
using retecs.ReteCs.core;

namespace retecs.Components
{
    public class TextControl : BlazorControl
    {
        public TextControl(Emitter emitter, string key, bool @readonly = false) : base(key, emitter)
        {
            Type = "string";
            ChangeHandler = OnChange;
            Value = string.Empty;
            Readonly = @readonly;
            Mounted = () =>
                      {
                          SetValue(GetData(key));
                      };
        }

        public void OnChange(object text)
        {
            Emitter.OnInfo("OnChange was called with ", text);

            if (text is string stringText)
            {
                Emitter.OnInfo("Changing Value to: " + stringText);
                SetValue(stringText);
            }
            else
            {
                Emitter.OnWarn(nameof(OnChange) + " was called with a value that can not be converted to string.");
                Emitter.OnWarn("Value was: " + JsonSerializer.Serialize(new {text}));
            }

            Emitter.OnInfo("Recalculating nodes, because a Control has a new value:", text);
            Emitter.OnProcess();
            Emitter.OnInfo("Recalculating nodes done!");
        }

        public void SetValue(object number)
        {
            Emitter.OnInfo("Write: " + JsonSerializer.Serialize(number) + "; To: " + Key);
            Value = number;
            PutData(Key, number);
        }
    }
}
