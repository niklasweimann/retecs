using Microsoft.AspNetCore.Components;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;

namespace retecs.ReteCs.View
{
    public class SocketView
    {
        public Emitter Emitter { get; set; }
        public ElementReference ElementReference { get; set; }
        public string Type { get; set; }
        public Io Io { get; set; }
        public Node Node { get; set; }

        public SocketView(ElementReference elementReference, string type, Io io, Node node, Emitter emitter)
        {
            Emitter = emitter;
            ElementReference = elementReference;
            Type = type;
            Io = io;
            Node = node;
            Emitter.OnRenderSocket(elementReference, io.Socket, io);
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