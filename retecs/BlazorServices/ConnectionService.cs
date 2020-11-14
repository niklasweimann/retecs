using Microsoft.AspNetCore.Components;
using retecs.ReteCs;
using retecs.ReteCs.core;

namespace retecs.BlazorServices
{
    public class ConnectionService
    {
        private Emitter Emitter { get; } = SingletonEmitter.Instance;

        private Input Input { get; set; }

        private Output Output { get; set; }

        private Socket InputSocket { get; set; }

        private Socket OutputSocket { get; set; }

        private ElementReference? InputElementReference { get; set; }

        private ElementReference? OutputElementReference { get; set; }

        public bool SetInput(Input input, Socket inputSocket, ElementReference inputElementReference)
        {
            if (!AreSocketsCompatible(inputSocket))
            {
                Emitter.OnError("Sockets are not compatible");
                return false;
            }

            Input = input;
            InputSocket = inputSocket;
            InputElementReference = inputElementReference;
            RenderConnection();
            return true;
        }

        public bool SetOutput(Output output, Socket outputSocket, ElementReference outputElementReference)
        {
            if (!AreSocketsCompatible(outputSocket))
            {
                Emitter.OnError("Sockets are not compatible");
                return false;
            }

            Output = output;
            OutputSocket = outputSocket;
            OutputElementReference = outputElementReference;
            RenderConnection();
            return true;
        }

        private void RenderConnection()
        {
            if (IsFirstSocket())
            {
                Emitter.OnDebug("First socket clicked. Return early");
                return;
            }

            Emitter.OnDebug($"Start rendering Connection {InputSocket == null} {OutputSocket == null}");
            if (InputElementReference == null ||
                OutputElementReference == null)
            {
                Emitter.OnError("Could not render Connection because a reference for a clicked Element is missing.");
                return;
            }
            Emitter.OnRenderConnection(new Connection(Output, Input), Input, Output, InputElementReference.Value,
                                       OutputElementReference.Value);
            Reset();
        }

        private bool IsFirstSocket()
        {
            Emitter.OnDebug($"Is first socket? InputSocket: {InputSocket != null} OutputSocket: {OutputSocket != null}");
            return InputSocket == null || OutputSocket == null;
        }

        private bool AreSocketsCompatible(Socket socketToAdd)
        {
            if (InputSocket != null)
            {
                return InputSocket.CompatibleWith(socketToAdd);
            }

            return OutputSocket == null || OutputSocket.CompatibleWith(socketToAdd);
        }

        private void Reset()
        {
            Emitter.OnInfo("Resetting Connection Service");
            Input = null;
            InputSocket = null;
            InputElementReference = null;
            Output = null;
            OutputSocket = null;
            OutputElementReference = null;
        }
    }
}
