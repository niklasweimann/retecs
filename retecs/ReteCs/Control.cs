using System;

namespace retecs.ReteCs
{
    public abstract class Control
    {
        public string Key { get; set; }
        public object Data { get; set; }
        public Node ParentNode { get; set; }
        public Input ParentInput { get; set; }

        public Control(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            Key = key;
        }

        public Node GetNode()
        {
            if (ParentInput?.Node != null && ParentInput != null)
            {
                throw new Exception("Control can\'t be added to Node and Input");
            }

            return ParentNode ?? ParentInput?.Node ?? throw new Exception("Control hasn\'t be added to Input or Node");
        }

        public object GetData(string key)
        {
            return GetNode().Data[key];
        }

        public void PutData(string key, object data)
        {
            GetNode().Data[key] = data;
        }
    }
}