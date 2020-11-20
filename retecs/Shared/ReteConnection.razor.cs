using System;
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
        private RenderFragment RenderFragment { get; set; }

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
            Emitter.NodeTranslated += (node, point) => Update();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            //if (InputElementReference != null &&
            //    OutputElementReference != null)
            {
                var points = GetPoints();
                if (points == null)
                {
                    return;
                }

                var d = DefaultPath(points.Value, 0.4);
                RenderFragment = RenderConnection(d, Connection);
            }
        }

        public ReteConnection(Connection connection, Input input, Output output, Emitter emitter)
        {
            Connection = connection;
            Input = input;
            Output = output;
            Emitter = emitter;
        }

        private (Point, Point)? GetPoints()
        {
            //if (InputElementReference == null ||
            //    OutputElementReference == null)
            //{
            //    return null;
            //}

           // Emitter.OnDebug($"InputElementReference: {InputElementReference == null}; OutputElementReference: {OutputElementReference == null}");

            var input = GetSocketPosition(InputElementReference);
            Emitter.OnDebug("Second Call");
            var output = GetSocketPosition(OutputElementReference);
            return (input, output);
        }

        public void Update()
        {
            var points = GetPoints();
            if (points == null)
            {
                return;
            }

            Emitter.OnDebug($"Point 1: {points.Value.Item1.X} {points.Value.Item1.Y} Point 2: {points.Value.Item2.X} {points.Value.Item2.Y}");
            var d = DefaultPath(points.Value, 0.4);
            Emitter.OnDebug("d is: " + d);
            RenderFragment = RenderConnection(d, Connection);
            Emitter.OnUpdateConnection(Connection, points.Value);
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

        private static RenderFragment RenderConnection(string d, Connection connection = null)
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

        private static string ToTrainCase(string str) => str.ToLower()
                                                            .Replace(' ', '-');

        private Point GetSocketPosition(ElementReference elementReference)
        {
            return BrowserService.GetPositionOfElement(elementReference);
        }
    }
}
