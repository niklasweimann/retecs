namespace retecs.ReteCs.Interfaces
{
    public interface IData
    {
        public string Id { get; set; }
        public INodeData Nodes { get; set; }
    }
}