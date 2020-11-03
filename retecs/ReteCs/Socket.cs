using System.Collections.Generic;

namespace retecs.ReteCs
{
    public class Socket
    {
        public string Name { get; set; }
        public object Data { get; set; }
        public List<Socket> Compatible { get; } = new List<Socket>();

        public Socket(string name, object data = null)
        {
            Name = name;
            Data = data;
        }

        public void CombineWith(Socket socket)
        {
            Compatible.Add(socket);
        }

        public bool CompatibleWith(Socket socket)
        {
            return this == socket || Compatible.Contains(socket);
        }
    }
}
