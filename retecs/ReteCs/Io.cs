using System.Collections.Generic;
using retecs.ReteCs.Interfaces;

namespace retecs.ReteCs
{
    public class Io
    {
        public Node Node { get; set; }
        public bool MultipleConnections { get; set; }
        public List<Connection> Connections { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public Socket Socket { get; set; }

        public Io(string key, string name, Socket socket, bool multipleConnections)
        {
            Node = null;
            MultipleConnections = multipleConnections;
            Connections = new List<Connection>();
            Key = key;
            Name = name;
            Socket = socket;
        }

        public void RemoveConnection(Connection connection)
        {
            Connections.Remove(connection);
        }

        public void RemoveAllConnections()
        {
            Connections.Clear();
        }
    }
}