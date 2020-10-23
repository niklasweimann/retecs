using System;

namespace retecs.ReteCs
{
    public class Output : Io
    {
        public Output(string key, string title, Socket socket, bool multiConns) : base(key, title, socket, multiConns)
        {
            
        }

        public bool HasConnection()
        {
            return Connections.Count > 0;
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
    }
}