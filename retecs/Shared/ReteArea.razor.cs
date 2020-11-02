using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;
using retecs.ReteCs.Enums;

namespace retecs.Shared
{
    public partial class ReteArea
    {
        [Inject]
        private Emitter Emitter { get; set; }
        public Transform Transform { get; } = new Transform();
        public Mouse Mouse { get; set; }

        private Transform _startPosition;

        public ReteArea()
        {
            
        }

        public ReteArea(Emitter emitter)
        {
            Emitter = emitter;

            Emitter.WindowMouseMove += PointerMove;

            Update();
        }

        public void Update()
        {
            // Transform
            GetStyles();
        }

        private string GetStyles()
        {
            var classes = new List<string>
            {
                "transform-origin: 0px 0px"
            };

            if (Transform != null)
            {
                classes.Add($"transform: translate({Transform.X}px, {Transform.Y}px) scale(1)");
            }
            return string.Join("; ", classes);
        }

        public void PointerMove(MouseEventArgs mouseEventArgs)
        {
            var clientX = mouseEventArgs.ClientX;
            var clientY = mouseEventArgs.ClientY;
            // TODO add BoundingClientRect
            var (left, top) = (0, 0);
            var x = clientX - left;
            var y = clientY - top;
            var k = Transform.K;

            Mouse = new Mouse(x / k, y / k);
            Emitter.OnMouseMove(Mouse);
        }


        private void OnStart()
        {
            _startPosition = Transform;
        }

        private void OnTranslate(Point point)
        {
            if (_startPosition != null)
            {
                Translate(_startPosition.X + point.X, _startPosition.Y + point.Y);
            }
        }

        private void Translate(double x, double y)
        {
            Emitter.OnTranslate(Transform, x, y);

            Transform.X = x;
            Transform.Y = y;

            Update();
            Emitter.OnTranslated();
        }

        private void OnZoom(double delta, int ox, int oy, ZoomSource source)
        {
            Zoom(Transform.K * (1 + delta), ox, oy, source);
            Update();
        }

        private void Zoom(double transformK, in int ox, in int oy, ZoomSource source)
        {
            var k = Transform.K;
            Emitter.OnZoom(Transform, transformK, source);

            var d = (k - transformK) / (k - transformK == 0 ? 1 : k - transformK);
            Transform.K = transformK == 0 ? 1 : transformK;
            Transform.X += ox * d;
            Transform.Y += oy * d;

            Update();
            Emitter.OnZoomed(source);
        }
    }
}