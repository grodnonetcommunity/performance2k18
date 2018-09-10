namespace ProtoEvents.Structs
{
    public class SimpleClass
    {
        public double A;
        public double B;
        public double C;
        public double D;
        public double E;
        public double F;
        public double G;
        public double H;

        public double Sum()
        {
            return A + H;
        }
    }

    public static class ClassHelpers
    {
        public static double CalculateSum(SimpleClass t)
        {
            return t.A + t.H;
        }

        public static double CalculateSum(ref SimpleClass t)
        {
            return t.A + t.H;
        }
    }
}