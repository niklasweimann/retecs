using System.Net.Sockets;
using Microsoft.AspNetCore.Components;
using retecs.ReteCs;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;

namespace retecs.Shared
{
    public partial class ReteSocket
    {
        public Emitter Emitter { get; set; }
        public string Type { get; set; }
        [Parameter]
        public Io Io { get; set; }
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
    }
}