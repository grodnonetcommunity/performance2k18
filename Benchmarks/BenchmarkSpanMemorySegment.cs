using System;
using System.Buffers;
using BenchmarkDotNet.Attributes;

namespace ProtoEvents
{
    [MemoryDiagnoser]
    public class BenchmarkSpanMemorySegment
    {
        private ArrayPool<int> _pool = ArrayPool<int>.Shared;
        private int[] _array;
        private Random _random = new Random();

        [GlobalSetup]
        public void Init()
        {
            _pool = ArrayPool<int>.Shared;
            _array = _pool.Rent(10_000);
            foreach (var i in _array)
            {
                _array[i] = _random.Next();
            }

        }

        [Benchmark]
        public int SpanSliceAndGet()
        {
            var slice = _array.AsSpan(0, 100);
            int last = -1;
            for (var i = 0; i < slice.Length; i++)
            {
                last = slice[i];
            }

            return last;
        }

        [Benchmark]
        public int MemorySliceAndGet()
        {
            var memory = _array.AsMemory(0, 100);
            var slice = memory.Span;
            int last = -1;
            for (var i = 0; i < slice.Length; i++)
            {
                last = slice[i];
            }

            return last;
        }

        [Benchmark]
        public int ArraySegmentSliceAndGet()
        {
            var slice = new ArraySegment<int>(_array, 0, 100);
            int last = -1;
            for (var i = 0; i < slice.Count; i++)
            {
                last = slice[i];
            }

            return last;
        }

        [Benchmark]
        public int ArraySliceAndGet()
        {
            var slice = new int[100];
            Array.Copy(_array, slice, 100);
            int last = -1;
            for (var i = 0; i < slice.Length; i++)
            {
                last = slice[i];
            }

            return last;
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _pool.Return(_array);
        }

    }
}