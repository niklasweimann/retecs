using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace retecs.ReteCs
{
    public class Input : Io
    {
        public Control Control { get; set; }

        public Input(string key, string title, Socket socket, bool multipleConntections) : base(key, title, socket,
            multipleConntections)
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
                throw new ArgumentException("Multiple Connections not allowed");
            }

            Connections.Add(connection);
        }

        public bool ShowControl()
        {
            return !HasConnection() && Control != null;
        }

        public string ToJson()
        {
            var res = "connections: [";
            foreach (var connection in Connections)
            {
                if (connection.Output.Node == null)
                {
                    throw new Exception("Node not added to Output");
                }

                res += JsonSerializer.Serialize(new
                {
                    Node = connection.Output.Node.Id,
                    Output = connection.Output.Key,
                    connection.Data
                });
            }

            res += "],";
            return res;
        }
    }
}