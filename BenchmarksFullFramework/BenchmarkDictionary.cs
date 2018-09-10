using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace ProtoEvents
{
    [MemoryDiagnoser]
    public class BenchmarkDictionary
    {
        private Dictionary<object, string> _objectDict = new Dictionary<object, string>();
        private Dictionary<Guid, string> _guidDictionary = new Dictionary<Guid, string>();
        private List<Guid> randomKey = new List<Guid>(5);
        private List<object> randomObjectKey = new List<object>(5);

        [GlobalSetup]
        public void Init()
        {
            var value = "Hello World!";
            var random = new Random();
            int[] randomIndexes = new int[5]
            {
                random.Next(10_000),
                random.Next(10_000),
                random.Next(10_000),
                random.Next(10_000),
                random.Next(10_000),
            };
            for (var i = 0; i < 10_000; i++)
            {

                var key = Guid.NewGuid();
                if (randomIndexes.Contains(i))
                {
                    randomKey.Add(key);
                    randomObjectKey.Add(key);
                }
                _guidDictionary[key] = value;
                _objectDict[key] = value;
            }


        }

        [Benchmark]
        public string GetByObject()
        {
            string result = null;
            for (var i = 0; i < randomObjectKey.Count; i++)
            {
                var key = randomObjectKey[i];
                result = _objectDict[key];
            }

            return result;
        }

        [Benchmark]
        public string GetByGuid()
        {
            string result = null;
            for (var i = 0; i < randomKey.Count; i++)
            {
                var key = randomKey[i];
                result = _guidDictionary[key];
            }
            return result;
        }
    }
}