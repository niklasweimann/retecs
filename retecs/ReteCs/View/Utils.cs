using System;
using retecs.ReteCs.JsInterop;

namespace retecs.ReteCs.View
{
    public class Utils
    {
        public static EventInterop EventInterop { get; } = new EventInterop();
        public static Action ListenWindow(string eventName, Action<object> action)
        {
            EventInterop.AddWindowEventListener(eventName, action);
            return () =>
            {
                //EventInterop.RemoveEventListener();
            };
        }
    }
}