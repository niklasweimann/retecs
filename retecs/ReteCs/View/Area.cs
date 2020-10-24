using Microsoft.AspNetCore.Components;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;
using retecs.ReteCs.Enums;
using retecs.ReteCs.JsInterop;

namespace retecs.ReteCs.View
{
    public class Area : Emitter
    {
        public ElementReference ElementReference { get; set; }
        public ElementReference Container { get; }
        public Transform Transform { get; set; }
        public Mouse Mouse { get; set; }
        public EventInterop EventInterop { get; } = new EventInterop();

        private Transform _startPosition;
        private Zoom _zoom;
        private Drag _drag;

        public Area(ElementReference container)
        {
            Container = container;

            /*
             TODO
             * const el = this.el = document.createElement('div');

                this.container = container;
                el.style.transformOrigin = '0 0';
             */

            _zoom = new Zoom(container, ElementReference, 0.1, OnZoom);
            _drag = new Drag(container, (point, _) => { OnTranslate(point); }, _ => OnStart());

            Destroy += HandleDestroy;

            EventInterop.AddEventListener(Container, "pointermove", _ =>  PointerMove());

            Update();
        }

        public void Update()
        {
            var t = Transform;
            //TODO
            // this.el.style.transform = `translate(${t.x}px, ${t.y}px) scale(${t.k})`;
        }

        public void PointerMove()
        {
            var clientX = 0;
            var clientY = 0;
            var (left, top) = (0, 0);
            var x = clientX - left;
            var y = clientY - top;
            var k = Transform.K;

            Mouse = new Mouse(x / k, y / k);
            OnMouseMove(Mouse);
        }


        private void OnStart()
        {
            _startPosition = Transform;
        }

        private void OnTranslate(Point point)
        {
            if (_zoom.Translating)
            {
                return;
            }

            if (_startPosition != null)
            {
                Translate(_startPosition.X + point.X, _startPosition.Y + point.Y);
            }
        }

        private void Translate(double x, double y)
        {
            base.OnTranslate(Transform, x, y);

            Transform.X = x;
            Transform.Y = y;

            Update();
            OnTranslated();
        }

        private void OnZoom(double delta, int ox, int oy, ZoomSource source)
        {
            Zoom(Transform.K * (1 + delta), ox, oy, source);
            Update();
        }

        private void Zoom(double transformK, in int ox, in int oy, ZoomSource source)
        {
            var k = Transform.K;
            base.OnZoom(Transform, transformK, source);

            var d = (k - transformK) / (k - transformK == 0 ? 1 : k - transformK);
            Transform.K = transformK == 0 ? 1 : transformK;
            Transform.X += ox * d;
            Transform.Y += oy * d;

            Update();
            OnZoomed(source);
        }

        public void AppendChild(ElementReference connViewHtmlElement)
        {
            EventInterop.AppendChild(ElementReference, connViewHtmlElement);
        }

        public void RemoveChild(ElementReference connViewHtmlElement)
        {
            EventInterop.RemoveChild(ElementReference, connViewHtmlElement);
        }

        public void HandleDestroy()
        {
            _zoom.Destroy();
            _drag.Destroy();
        }
    }
}