using BenchmarkDotNet.Attributes;

namespace ProtoEvents
{
    /*
     * This code was directly stolen from Adam Sitnik blog
     * https://adamsitnik.com/Value-Types-vs-Reference-Types/
     */
    public class BenchmarkValueTypeInvokingInterfaceMethod
    {
        interface IInterface
        {
            void DoNothing();
        }

        class ReferenceTypeImplementingInterface : IInterface
        {
            public void DoNothing() { }
        }

        struct ValueTypeImplementingInterface : IInterface
        {
            public void DoNothing() { }
        }

        private ReferenceTypeImplementingInterface reference = new ReferenceTypeImplementingInterface();
        private ValueTypeImplementingInterface value = new ValueTypeImplementingInterface();

        [Benchmark(Baseline = true)]
        public void ValueType() => AcceptingInterface(value);

        [Benchmark]
        public void ReferenceType() => AcceptingInterface(reference);

        void AcceptingInterface(IInterface instance) => instance.DoNothing();
    }
}