using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace retecs.RazorUtils
{
    public class BrowserService
    {
        [Inject]
        private IJSInProcessRuntime JsRuntime { get; }

        public BrowserService(IJSRuntime inProcessRuntime)
        {
            JsRuntime = (IJSInProcessRuntime)inProcessRuntime;
        }

        public BrowserDimension GetDimension()
        {
            return JsRuntime.Invoke<BrowserDimension>("getDimensions");
        }
    }

    public class BrowserDimension
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}