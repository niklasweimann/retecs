using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using retecs.BlazorServices;
using retecs.ReteCs;
using retecs.ReteCs.core;
using retecs.ReteCs.Enums;

namespace retecs.Shared
{
    public partial class ReteSocket
    {
        [Inject]
        public ConnectionService ConnectionService { get; set; }
        [Inject]
        public Emitter Emitter { get; set; }
        
        [Parameter]
        public IoTypes Type { get; set; }
        [Parameter]
        public Io Io { get; set; }
        [Parameter]
        public Socket Socket { get; set; }
        public Node Node { get; set; }

        public ElementReference? SocketDot { get; set; }


        private string SocketClasses()
        {
            var classes = new List<string> { "socket" };

            if (Socket != null)
            {
                classes.Add(Socket.Name);
            }

            if (Io != null)
            {
                classes.Add(Io is Input ? "input" : "output");
            }
            return string.Join(" ", classes);
        }

        private void HandleSocketClick(MouseEventArgs mouseEventArgs)
        {
            if (Io.GetType().IsAssignableFrom(typeof(Input)))
            {
                if (!ConnectionService.SetInput((Input) Io, Socket, SocketDot.Value))
                {
                    Emitter.OnWarn("Could not creat Connection");
                }
            }
            else if (Io.GetType().IsAssignableFrom(typeof(Output)))
            {
                if (!ConnectionService.SetOutput((Output) Io, Socket, SocketDot.Value))
                {
                    Emitter.OnWarn("Could not create Connection");
                }
            }
            else
            {
                Emitter.OnError("Socket is neither input nor output");
            }
        }
    }
}