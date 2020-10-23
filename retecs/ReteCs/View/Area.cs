using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;
using retecs.ReteCs.Enums;

namespace retecs.ReteCs.View
{
    public class Area : Emitter
    {
        public RenderFragment ElRenderFragment { get; set; }
        public ElementReference Container { get; set; }
        public Transform Transform { get; set; }
        public Mouse Mouse { get; set; }

        private Transform _startPosition;
        private Zoom _zoom;
        private Drag _drag;

        public Area(ElementReference container)
        {
            Container = container;
            ElRenderFragment = builder =>
            {
                builder.OpenElement(0, "div");
                // el.style.transformOrigin = '0 0';
            };

            _zoom = new Zoom(container, ElRenderFragment, 0.1,
                (delta, ox, oy, source) => { OnZoom(delta, ox, oy, source); });
            _drag = new Drag(container, (x, y, _) => { OnTranslate(x, y); }, (_) =>  OnStart());

            On(new List<string> {"destroy"}, _ =>
            {
                _zoom.Destroy();
                _drag.Destroy();
                return true;
            });

            //Todo: JSRuntime interop
            //this.container.addEventListener('pointermove', this.pointermove.bind(this));

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

        private void OnTranslate(double dx, double dy)
        {
            if (_zoom.Translating)
            {
                return;
            }

            if (_startPosition != null)
            {
                Translate(_startPosition.X + dx, _startPosition.Y + dy);
            }
        }

        private void Translate(double x, double y)
        {
            // TODO create Type
            var parameters = new {Transform, x, y};
            if (!Trigger("translate", parameters))
            {
                return;
            }

            Transform.X = parameters.x;
            Transform.Y = parameters.y;

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
            if (!Trigger("zoom", Transform, transformK, source))
            {
                return;
            }

            var d = (k - transformK) / (k - transformK == 0 ? 1 : k - transformK);
            Transform.K = transformK == 0 ? 1 : transformK;
            Transform.X += ox * d;
            Transform.Y += oy * d;

            Update();
            OnZoomed(source);
        }

        public void AppendChild()
        {
            throw new NotImplementedException();
        }

        public void RemoveChild()
        {
            throw new NotImplementedException();
        }
    }
}