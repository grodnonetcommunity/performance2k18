namespace ProtoEvents.Structs
{
    public class SimpleStruct
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

    public static class StructHelpers
    {
        public static double CalculateSum(SimpleStruct t)
        {
            return t.A + t.H;
        }

        public static double CalculateSum(ref SimpleStruct t)
        {
            return t.A + t.H;
        }
    }
}