using Microsoft.AspNetCore.Components;
using retecs.ReteCs;
using retecs.ReteCs.core;

namespace retecs.BlazorServices
{
    public class ConnectionService
    {
        public Emitter Emitter { get; } = SingletonEmitter.Instance;

        public Input Input { get; private set; }
        public Output Output { get; private set; }

        public Socket InputSocket { get; private set; }
        public Socket OutputSocket { get; private set; }
        public ElementReference InputElementReference { get; set; }
        public ElementReference OutputElementReference { get; set; }

        public bool SetInput(Input input, Socket inputSocket, ElementReference inputElementReference)
        {
            if (Output == null && Input != null)
            {
                Reset();
                Emitter.OnInfo("Canceling connection, because input clicked two times.");
                return false;
            }
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
            if (Output != null && Input == null)
            {
                Reset();
                Emitter.OnInfo("Canceling connection, because output clicked two times.");
                return false;
            }
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
            Emitter.OnRenderConnection(new Connection(Output, Input), Input, Output, InputElementReference, OutputElementReference );
            Reset();
        }

        private bool IsFirstSocket() => InputSocket == null || OutputSocket == null;

        private bool AreSocketsCompatible(Socket socketToAdd) =>
            InputSocket?.CompatibleWith(socketToAdd) ?? OutputSocket == null || OutputSocket.CompatibleWith(socketToAdd);

        private void Reset()
        {
            Input = null;
            Output = null;
        }
    }
}
