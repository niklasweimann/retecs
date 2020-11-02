using Microsoft.AspNetCore.Components;
using retecs.ReteCs;
using retecs.ReteCs.core;

namespace retecs.BlazorServices
{
    public class ConnectionService
    {
        [Inject]
        public Emitter Emitter { get; set; }

        public Input Input { get; private set; }
        public Output Output { get; private set; }

        public Socket InputSocket { get; private set; }
        public Socket OutputSocket { get; private set; }

        public bool SetInput(Input input, Socket inputSocket)
        {
            if (!AreSocketsCompatible())
            {
                Emitter.OnError("Sockets are not compatible");
                return false;
            }
            Input = input;
            InputSocket = inputSocket;
            RenderConnection();
            // TODO raise event for input set
            return true;
        }

        public bool SetOutput(Output output, Socket outputSocket)
        {
            if (!AreSocketsCompatible())
            {
                Emitter.OnError("Sockets are not compatible");
                return false;
            }
            Output = output;
            OutputSocket = outputSocket;
            RenderConnection();
            // todo raise event for output set
            return true;
        }

        private void RenderConnection()
        {
            if (IsFirstSocket()) return;
            Emitter.OnRenderConnection(new Connection(Output, Input), Input, Output);
            Input = null;
            Output = null;
        }

        private bool IsFirstSocket()
        {
            return InputSocket == null && OutputSocket == null;
        }

        private bool AreSocketsCompatible()
        {
            return InputSocket.CompatibleWith(OutputSocket);
        }

        public void Reset()
        {
            Input = null;
            Output = null;
        }
    }
}