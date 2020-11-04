using retecs.ReteCs;

namespace retecs.Components
{
    public class Sockets
    {
        public static Socket NumberSocket { get; set; } = new Socket("Number value");
        public static Socket TextSocket { get; set; } = new Socket("Text value");
    }
}
