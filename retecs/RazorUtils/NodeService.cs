using System;
using System.Net.Sockets;
using Microsoft.AspNetCore.Components;
using retecs.ReteCs;

namespace retecs.RazorUtils
{
    public class NodeService
    {
        public Action<string, Io> BindSocket { get; set; }
        public Action<ElementReference, Control> BindControl { get; set; }

        public void SetBindings(Action<string, Io> bindSocket,
            Action<ElementReference, Control> bindControl)
        {
            BindSocket = bindSocket;
            BindControl = bindControl;
        }
    }
}