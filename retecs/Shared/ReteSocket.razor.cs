using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using retecs.BlazorServices;
using retecs.ReteCs;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;

namespace retecs.Shared
{
    public partial class ReteSocket
    {
        [Inject]
        public ConnectionService ConnectionService { get; set; }
        public Emitter Emitter { get; set; }
        public string Type { get; set; }
        [Parameter]
        public Io Io { get; set; }

        [Parameter]
        public Socket Socket { get; set; }

        public Node Node { get; set; }


        public ReteSocket()
        {
            
        }

        public ReteSocket(string type, Io io, Node node, Emitter emitter)
        {
            Emitter = emitter;
            Type = type;
            Io = io;
            Node = node;
            Emitter.OnRenderSocket(io.Socket, io);
        }

        public Point GetPosition(Point position)
        {
            // TODO: Get ElementReference offsetLeft and offsetWidth and offsetTop and offsetHeight
            /*
             * return [
            position[0] + el.offsetLeft + el.offsetWidth / 2,
            position[1] + el.offsetTop + el.offsetHeight / 2
             */
            return new Point(1, 1);
        }

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
                if (!ConnectionService.SetInput((Input) Io, Socket))
                {
                    Emitter.OnWarn("Could not creat Connection");
                }
            }
            else if (Io.GetType().IsAssignableFrom(typeof(Output)))
            {
                if (!ConnectionService.SetOutput((Output) Io, Socket))
                {
                    Emitter.OnWarn("Could not create Connection");
                }
            }
            else
            {
                Emitter.OnError("Socket is neither input nor output");
            }
            Emitter.OnInfo($"Emitter is null? {ConnectionService.Emitter == null}");

        }
    }
}