using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using retecs.ReteCs.core;

namespace retecs.ReteCs.View
{
    public class SocketView : Emitter
    {
        public ElementReference ElementReference { get; set; }
        public string Type { get; set; }
        public Io Io { get; set; }
        public Node Node { get; set; }

        public SocketView(ElementReference elementReference, string type, Io io, Node node)
        {
            ElementReference = elementReference;
            Type = type;
            Io = io;
            Node = node;
            OnRenderSocket(elementReference, io.Socket, io);
        }

        public (double posX, double posY) GetPosition((double posX, double posY) position)
        {
            // TODO: Get ElementReference offsetLeft and offsetWidth and offsetTop and offsetHeight
            /*
             * return [
            position[0] + el.offsetLeft + el.offsetWidth / 2,
            position[1] + el.offsetTop + el.offsetHeight / 2
             */
            return (1, 1);
        }
    }
}