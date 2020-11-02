using System;
using System.Linq;
using retecs.ReteCs.Entities;

namespace retecs.ReteCs
{
    public class Input : Io
    {
        public Control Control { get; set; }

        public Input(string key, string title, Socket socket, bool multipleConnections = false) : base(key, title, socket,
            multipleConnections)
        {

        }

        public bool HasConnection()
        {
            return Connections.Any();
        }

        public void AddConnection(Connection connection)
        {
            if (!MultipleConnections && HasConnection())
            {
                throw new ArgumentException("Multiple ConnectionViews not allowed");
            }

            Connections.Add(connection);
        }

        public void AddControl(Control control)
        {
            Control = control;
            control.ParentInput = this;
        }

        public bool ShowControl()
        {
            return !HasConnection() && Control != null;
        }

        public InputData ToJson()
        {
            return new InputData
            {
                Connections = Connections?.Select(x => new InputConnectionData
                {
                    Node = x.Output.Node.Id, Output = x.Output.Key, Data = x.Data
                }).ToList()
            };
        }
    }
}