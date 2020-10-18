namespace retecs.ReteCs.Engine
{
    public class Component
    {
        public string Name { get; set; }
        public object Data { get; set; }
        public Engine Engine { get; set; }

        public Component(string name)
        {
            Name = name;
        }
    }
}