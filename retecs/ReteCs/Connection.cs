namespace retecs.ReteCs
{
    public class Connection
    {
        public Output Output { get; set; }
        public Input Input { get; set; }
        public object Data { get; set; }
        public Connection(Output output, Input input)
        {
            Output = output;
            Input = input;
            Data = null;
            Input.AddConnection(this);
        }

        public void Remove()
        {
            Input.RemoveConnection(this);
            Output.RemoveConnection(this);
        }
    }
}