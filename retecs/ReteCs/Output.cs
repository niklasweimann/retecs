using System;
using System.Linq;
using retecs.ReteCs.Entities;

namespace retecs.ReteCs
{
    public class Output : Io
    {
        public Output(string key, string title, Socket socket, bool multiConns = false) : base(key, title, socket, multiConns)
        {
            
        }

        public bool ConnectedTo(Input input)
        {
            return Connections.Any(x => x.Input == input);
        }

        public Connection ConnectTo(Input input)
        {
            if (!Socket.CompatibleWith(input.Socket))
            {
                throw new Exception("Sockets not compatible");
            }

            if (!input.MultipleConnections && input.HasConnection())
            {
                throw new Exception("Input already has one connection");
            }

            if (!MultipleConnections && HasConnection())
            {
                throw new Exception("Output already has one connection");
            }
            
            var connection = new Connection(this, input);
            Connections.Add(connection);
            return connection;
        }

        public OutputData ToJson()
        {
            return new OutputData
            {
                Connections = Connections?.Select(x => new OutputConnectionData
                {
                    Node = x.Input.Node.Id, Input = x.Input.Key, Data = x.Data
                }).ToList()
            };
        }
    }
}