using System.Buffers;
using BenchmarkDotNet.Attributes;

namespace ProtoEvents
{
    [MemoryDiagnoser]
    public class BenchmarkPool
    {
        [Benchmark]
        public object[] AllocateArray100()
        {
            return new object[100];
        }

        [Benchmark]
        public object[] AllocateArray1000()
        {
            return new object[1000];
        }

        [Benchmark]
        public object[] AllocateArray10_000()
        {
            return new object[10_000];
        }

        [Benchmark]
        public void RentAndReturn100()
        {
            var array = ArrayPool<object>.Shared.Rent(100);
            ArrayPool<object>.Shared.Return(array);
        }

        [Benchmark]
        public void RentAndReturn1000()
        {
            var array = ArrayPool<object>.Shared.Rent(1000);
            ArrayPool<object>.Shared.Return(array);
        }

        [Benchmark]
        public void RentAndReturn10_000()
        {
            var array = ArrayPool<object>.Shared.Rent(10000);
            ArrayPool<object>.Shared.Return(array);
        }
    }
}