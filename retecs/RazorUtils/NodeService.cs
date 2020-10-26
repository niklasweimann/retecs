using System;
using System.Net.Sockets;
using Microsoft.AspNetCore.Components;
using retecs.ReteCs;

namespace retecs.RazorUtils
{
    public class NodeService
    {
        public Action<ElementReference, SocketType, Io> BindSocket { get; set; }
        public Action<ElementReference, Control> BindControl { get; set; }

        public void SetBindings(Action<ElementReference, SocketType, Io> bindSocket,
            Action<ElementReference, Control> bindControl)
        {
            BindSocket = bindSocket;
            BindControl = bindControl;
        }
    }
}