using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using retecs.ReteCs.Enums;

namespace retecs.ReteCs.View
{
    public class Zoom
    {
        public Action Destroy;
        public PointerEventArgs[] Pointers;

        public Zoom(ElementReference container, RenderFragment elRenderFragment, double d,
            Action<double, int, int, ZoomSource> action)
        {
            throw new NotImplementedException();
        }

        public bool Translating => Pointers.Length > 2;
    }
}