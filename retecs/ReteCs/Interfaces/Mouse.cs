namespace retecs.ReteCs.Interfaces
{
    public class Mouse
    {
        public Mouse(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }
}