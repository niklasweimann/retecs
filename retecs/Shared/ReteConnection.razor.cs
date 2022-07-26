﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using retecs.BlazorServices;
using retecs.ReteCs;
using retecs.ReteCs.core;
using retecs.ReteCs.Entities;

namespace retecs.Shared
{
    public partial class ReteConnection
    {
        public RenderFragment RenderFragment { get; set; }
        [Inject]
        private Emitter Emitter { get; set; }
        [Inject]
        public BrowserService BrowserService { get; set; }
        [Parameter]
        public Connection Connection { get; set; }
        [Parameter]
        public Input Input { get; set; }
        [Parameter]
        public Output Output { get; set; }
        [Parameter]
        public ElementReference InputElementReference { get; set; }
        [Parameter]
        public ElementReference OutputElementReference { get; set; }

        public ReteConnection()
        {
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Emitter.NodeTranslated += (a, b) => Update();
        }

        public ReteConnection(Connection connection, Input input, Output output, Emitter emitter)
        {
            Connection = connection;
            Input = input;
            Output = output;
            Emitter = emitter;
        }

        public (Point,Point) GetPoints()
        {
            return (GetSocketPosition(InputElementReference), GetSocketPosition(OutputElementReference));
        }

        public void Update()
        {
            var points = GetPoints();
            Emitter.OnDebug($"Point 1: {points.Item1.X} {points.Item1.Y} Point 2: {points.Item2.X} {points.Item2.Y}");
            var d = DefaultPath(points, 0.4);
            Emitter.OnDebug("d is: " + d);
            RenderFragment = RenderConnection(d, Connection);
            Emitter.OnUpdateConnection(Connection, GetPoints());
            Emitter.OnDebug("Connection updated!");
        }

        public static string DefaultPath((Point start, Point end) points, double curvature)
        {
            var start = points.start;
            var end = points.end;

            var hx1 = start.X + Math.Abs(end.X - start.X) * curvature;
            var hx2 = end.X - Math.Abs(end.X - start.X) * curvature;

            return $"M {start.X} {start.Y} C {hx1} {start.Y} {hx2} {end.Y} {end.X} {end.Y}";
        }

        public static string RenderPathData(Emitter emitter, (Point start, Point end) points, Connection connection = null)
        {
            var connectionPath = DefaultPath(points, 0.4);
            emitter.OnConnectionPath(points, connection, connectionPath);
            return connectionPath ?? string.Empty;
        }

        public static RenderFragment RenderConnection(string d, Connection connection = null)
        {
            var classes = new List<string>();
            if (connection != null)
            {
                classes.Add("input-" + ToTrainCase(connection.Input.Name));
                classes.Add("output-" + ToTrainCase(connection.Output.Name));
                classes.Add("socket-input-" + ToTrainCase(connection.Input.Socket.Name));
                classes.Add("socket-output-" + ToTrainCase(connection.Output.Socket.Name));
            }

            var seq = 0;

            void Path(RenderTreeBuilder builder)
            {
                builder.OpenElement(++seq, "svg");
                builder.AddAttribute(++seq, "class", string.Join(" ", classes));
                builder.OpenElement(++seq, "path");
                builder.AddAttribute(++seq, "class", "main-path");
                builder.AddAttribute(++seq, "d", d);
                builder.CloseElement();
                builder.CloseElement();
            }

            return Path;
        }

        public static string ToTrainCase(string str)
        {
            return str.ToLower().Replace(' ', '-');
        }


        public Point GetSocketPosition(ElementReference elementReference)
        {
            return BrowserService.GetPositionOfElement(elementReference);
        }
    }
}