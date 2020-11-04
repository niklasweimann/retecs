using System.Collections.Generic;
using System.Linq;
using Cryptool.Caesar;
using retecs.ReteCs;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;

namespace retecs.Components
{
    public class CaesarComponent : Component
    {
        public Emitter Emitter { get; }

        public CaesarComponent(Emitter emitter) : base("Caesar")
        {
            Emitter = emitter;
        }

        public override void Worker(
            NodeData node,
            Dictionary<string, List<object>> inputs,
            Dictionary<string, object> outputs,
            params object[] args)
        {
            // Text input
            inputs.TryGetValue("input", out var inputList);
            var input = inputList.FirstOrDefault() ?? node.Data["num1"];

            // Rotation input
            inputs.TryGetValue("rot", out var rotList);
            var rot = rotList.FirstOrDefault() ?? node.Data["num2"];

            var caesar = new Caesar();
            caesar.Settings.AlphabetSymbols = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            caesar.Settings.Action = CaesarSettings.CaesarMode.Encrypt;
            caesar.Settings.CaseSensitive = false;
            caesar.Settings.ShiftKey = (int)rot;
            caesar.Settings.UnknownSymbolHandling = CaesarSettings.UnknownSymbolHandlingMode.Ignore;

            caesar.Execute();

            var editorNode = Editor.Nodes.FirstOrDefault(n => n.Id == node.Id);

            if (editorNode == null)
            {
                Emitter.OnDebug($"editorNode is null, so return early");
                return;
            }
            caesar.PropertyChanged += (sender, eventArgs) =>
                                      {
                                          if (eventArgs.PropertyName != nameof(caesar.OutputString))
                                          {
                                              return;
                                          }

                                          editorNode.Controls.TryGetValue("preview", out var control1);
                                          Emitter.OnDebug($"control is null? {control1 == null}");
                                          ((TextControl) control1)?.SetValue(caesar.OutputString ?? string.Empty);

                                          outputs["cypher"] = caesar.OutputString;
                                      };
            editorNode.Controls.TryGetValue("preview2", out var control2);
            Emitter.OnDebug($"control2 is null? {control2 == null}");
            ((TextControl) control2)?.SetValue(input ?? string.Empty);
        }

        public override void Builder(Node node)
        {
            var inp1 = new Input("rot", "Rotation", Sockets.NumberSocket);
            var inp2 = new Input("input", "Text", Sockets.TextSocket);
            var output = new Output("cypher", "Cypher-Text", Sockets.TextSocket);

            inp1.AddControl(new TextControl(Emitter, "num1"));
            inp2.AddControl(new TextControl(Emitter, "num2"));

            node.AddInput(inp1)
                .AddInput(inp2)
                .AddControl(new TextControl(Emitter, "preview", true))
                .AddControl(new TextControl(Emitter, "preview2", true))
                .AddOutput(output);
        }
    }
}
