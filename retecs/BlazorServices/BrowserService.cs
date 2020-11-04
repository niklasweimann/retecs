using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using retecs.ReteCs.Entities;

namespace retecs.BlazorServices
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
            return JsRuntime.Invoke<BrowserDimension>("ReteCsInterop.getDimensions");
        }

        public Point GetPositionOfElement(ElementReference elementReference)
        {
            var pos = JsRuntime.Invoke<int[]>("ReteCsInterop.getPosition", elementReference);
            return new Point(pos[0], pos[1]);
        }

        public (Point, Point) GetBoundingBox(ElementReference elementReference)
        {
            var positions = JsRuntime.Invoke<int[]>("ReteCsInterop.getBoundingBox", elementReference);
            return (new Point(positions[0], positions[1]), new Point(positions[2], positions[3]));
        }
    }

    public class BrowserDimension
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
