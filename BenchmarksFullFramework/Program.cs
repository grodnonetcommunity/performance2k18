using BenchmarkDotNet.Running;
using ProtoEvents;

namespace BenchmarksFullFramework
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<BenchmarkDictionary>();
        }
    }
}