using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using ProtoEvents.Structs;

namespace ProtoEvents
{
    [MemoryDiagnoser]
    public class BenchmarkStruct
    {
        private SimpleStruct _ss;
        private SimpleClass _sc;

        [GlobalSetup]
        public void Init()
        {
            _ss = new SimpleStruct
            {
                A = 1.5,
                B = 2.5,
                C = 1.0,
                D = 4.5,
                E = 1.5,
                F = 2.5,
                G = 1.0,
                H = 4.5
            };

            _sc = new SimpleClass
            {
                A = 1.5,
                B = 2.5,
                C = 1.0,
                D = 4.5,
                E = 1.5,
                F = 2.5,
                G = 1.0,
                H = 4.5
            };
        }

        [Benchmark]
        public double StructSumInstanceMethod()
        {
            return _ss.Sum();
        }

        [Benchmark]
        public double StructSumStaticMethodByRef()
        {
            return StructHelpers.CalculateSum(ref _ss);
        }

        [Benchmark]
        public double StructSumStaticMethod()
        {
            return StructHelpers.CalculateSum(_ss);
        }

        [Benchmark]
        public double ClassSumInstanceMethod()
        {
            return _sc.Sum();
        }

        [Benchmark]
        public double ClassSumStaticMethodByRef()
        {
            return ClassHelpers.CalculateSum(ref _sc);
        }

        [Benchmark]
        public double ClassSumStaticMethod()
        {
            return ClassHelpers.CalculateSum(_sc);
        }
    }
}