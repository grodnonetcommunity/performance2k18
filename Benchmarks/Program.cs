using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Jil;
using ProtoBuf;

namespace ProtoEvents
{
    class Program
    {
        private static Random random = new Random();

        static async Task Main(string[] args)
        {
            /*var settings = ConnectionSettings.Create().SetDefaultUserCredentials(new UserCredentials("admin", "changeit"));
            //var connection = EventStoreConnection.Create(settings, new IPEndPoint(IPAddress.Loopback, 1113));

            //await connection.ConnectAsync();

            for (int i = 0; i < 10_000; i++)
            {
                var @event = new TestEvent(Guid.NewGuid(), RandomString(1024));
                var result = ProtobufSerializer(@event);
                var result1 = ProtoDeserializer(result);
                //await connection.AppendToStreamAsync("Test", i - 1,
                //    new EventData(@event.Id, "TestEvent", true, ProtobufSerializer(@event), null));
            }*/
            // var setMethod = typeof(TestEvent).GetProperty(nameof(TestEvent.Id)).GetSetMethod(true);
            BenchmarkRunner.Run<BenchmarkArray>();
            // BenchmarkRunner.Run<BenchmarkIterations>();

            /*b.CreateTestData();
            b.DeserializeMsgPack();
            b.DeserializeGProto();*/
            /*var tre = new TestReadonlyEvent(Guid.NewGuid(), "asdasdadsa", 2.333, -73.222, 23.4413, DateTimeOffset.UtcNow);
            var text = JSON.Serialize(tre, Options.ISO8601ExcludeNullsUtcCamelCase);
            var tst2 = JSON.Deserialize<TestReadonlyEvent>(text, Options.ISO8601ExcludeNullsUtcCamelCase);*/

        }


    }
}