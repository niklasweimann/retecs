using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace retecs.ReteCs.JsInterop
{
    public class EventInterop
    {
        public static IJSRuntime JsRuntime { get; set; }
        private Dictionary<string, Action<object>> CallbackRef { get; } = new Dictionary<string, Action<object>>();

        public async Task AddEventListener(ElementReference container, string eventName, Action<object> action)
        {
            var callbackRef = Guid.NewGuid().ToString();
            CallbackRef.Add(callbackRef, action);
            var dotNetReference = DotNetObjectReference.Create(this);

            await JsRuntime.InvokeVoidAsync("ReteCsInterop.addEventListener", container, eventName, callbackRef,
                dotNetReference);
        }
        
        public async Task AddWindowEventListener(string eventName, Action<object> action)
        {
            var callbackRef = Guid.NewGuid().ToString();
            CallbackRef.Add(callbackRef, action);
            var dotNetReference = DotNetObjectReference.Create(this);

            await JsRuntime.InvokeVoidAsync("ReteCsInterop.addWindowEventListener", eventName, callbackRef,
                dotNetReference);
        }


        [JSInvokable("ReturnEventCallback")]
        public void ReturnEventCallback(string callbackRef, object @event)
        {
            CallbackRef.TryGetValue(callbackRef, out var callbackAction);
            callbackAction?.Invoke(@event);
        }

        //public void RemoveEventListener(ElementReference container, string eventName)
        //{
        //    JsRuntime.InvokeVoidAsync("ReteCsInterop.removeEventListener", container, eventName, );
        //}

        public void AppendChild(ElementReference parentReference, ElementReference childReference)
        {
            JsRuntime.InvokeVoidAsync("ReteCsInterop.appendChild", parentReference, childReference);
        }

        public void RemoveChild(ElementReference parentReference, ElementReference childReference)
        {
            JsRuntime.InvokeVoidAsync("ReteCsInterop.removeChild", parentReference, childReference);
        }
    }
}