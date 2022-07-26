﻿using System;
using Microsoft.AspNetCore.Components.Web;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;

namespace retecs.Shared
{
    public class Drag
    {
        public Point PointerStart { get; set; }
        public Emitter Emitter { get; }
        public Action<MouseEventArgs> OnStart { get; }
        public Action<MouseEventArgs> OnDrag { get; }
        public Action Destroy { get; }

        public Action<Point, MouseEventArgs> OnTranslate { get; }

        public Drag(Emitter emitter, Action<Point, MouseEventArgs> onTranslate,
            Action<MouseEventArgs> onStart, Action<MouseEventArgs> onDrag = null)
        {
            PointerStart = null;
            OnStart = onStart;
            OnDrag = onDrag;
            OnTranslate = onTranslate;
            Emitter = emitter;

            Emitter.WindowMouseDown += Down;
            Emitter.WindowMouseMove += Move;
            Emitter.WindowMouseUp += Up;

            Destroy = () =>
            {
                Emitter.WindowMouseMove -= Move;
                Emitter.WindowMouseUp -= Up;
            };
        }

        public void Down(MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Type == "mouse" && mouseEventArgs.Button != 0)
            {
                return;
            }

            PointerStart = new Point(mouseEventArgs.ClientX, mouseEventArgs.ClientY);
            OnStart(mouseEventArgs);
        }

        public void Move(object eventArgs)
        {
            var mouseEventArgs = (MouseEventArgs)eventArgs;
            if (PointerStart == null)
            {
                return;
            }

            var x = mouseEventArgs.ClientX;
            var y = mouseEventArgs.ClientY;

            var deltaX = x - PointerStart.X;
            var deltaY = y - PointerStart.Y;

            // TODO get getBoundingClientRect via Javascript
            var zoom = 1;
            //var zoom = this.el.getBoundingClientRect().width / this.el.offsetWidth;
            OnTranslate(new Point(deltaX / zoom, deltaY / zoom), mouseEventArgs);
        }

        public void Up(object eventArgs)
        {
            var mouseEventArgs = (MouseEventArgs)eventArgs;
            if (PointerStart == null)
            {
                return;
            }

            PointerStart = null;
            OnDrag(mouseEventArgs);
        }
    }
}