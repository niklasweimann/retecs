using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
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
        [Parameter]
        public Connection Connection { get; set; }
        [Parameter]
        public ReteNode InputNode { get; set; }
        [Parameter]
        public ReteNode OutputNode { get; set; }

        public ReteConnection()
        {
        }

        public ReteConnection(Connection connection, ReteNode viewInput, ReteNode viewOutput, Emitter emitter)
        {
            Connection = connection;
            InputNode = viewInput;
            OutputNode = viewOutput;
            Emitter = emitter;
        }

        public (Point,Point) GetPoints()
        {
            return (OutputNode.GetSocketPosition(Connection.Output), OutputNode.GetSocketPosition(Connection.Input));
        }

        public void Update()
        {
            var points = (InputNode.GetSocketPosition(Connection.Input), OutputNode.GetSocketPosition(Connection.Output));
            var d = DefaultPath(points, 0.4);
            RenderFragment = RenderConnection(d, Connection);
            Emitter.OnUpdateConnection(Connection, GetPoints());
        }

        public static string DefaultPath((Point start, Point end) points, double curvature)
        {
            var start = points.start;
            var end = points.end;

            var hx1 = start.X + Math.Abs(end.X - start.X) * curvature;
            var hx2 = end.X - Math.Abs(end.X - start.X) * curvature;

            return $"M {start.X} {start.Y} C {hx1} {start.Y} {hx2} {end.Y} {end.X} {end.Y}";
        }

        public static string RenderPathData(Emitter emitter, (Point start, Point end) points, ReteCs.Connection connection = null)
        {
            var connectionPath = DefaultPath(points, 0.4);
            emitter.OnConnectionPath(points, connection, connectionPath);
            return connectionPath ?? string.Empty;
        }

        public static RenderFragment RenderConnection(string d, ReteCs.Connection connection = null)
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
            RenderFragment path = builder =>
            {
                builder.OpenElement(++seq, "svg");
                builder.AddAttribute(++seq, "class", string.Join(" ", classes));
                builder.OpenElement(++seq, "path");
                builder.AddAttribute(++seq, "class", "main-path");
                builder.AddAttribute(++seq, "d", d);
                builder.CloseElement();
                builder.CloseElement();
            };

            return path;
        }

        public static string ToTrainCase(string str)
        {
            return str.ToLower().Replace(' ', '-');
        }
    }
}