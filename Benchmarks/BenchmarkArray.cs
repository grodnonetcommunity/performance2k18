using System;
using System.IO;
using System.Linq;
using System.Text;
using Avro.IO;
using Avro.Specific;
using BenchmarkDotNet.Attributes;
using Bond.IO.Safe;
using Bond.Protocols;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Jil;
using MessagePack;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProtoBuf;
using ProtoBuf.Meta;
using SpanJson.Resolvers;
using Utf8Json.Resolvers;
using ZeroFormatter;

namespace ProtoEvents
{
    [MemoryDiagnoser]
    public class BenchmarkArray
    {
        private static Random random = new Random();

        private static SpecificDatumWriter<Avro.TestEventArray> avroSerializer =
            new SpecificDatumWriter<Avro.TestEventArray>(Avro.TestEventArray._SCHEMA);

        private static SpecificDatumReader<Avro.TestEventArray> _avroDeserializer =
            new SpecificDatumReader<Avro.TestEventArray>(Avro.TestEventArray._SCHEMA, Avro.TestEventArray._SCHEMA);

        private TestEventArray _events = new TestEventArray();
        private EventArrayMsgpackIntKeys _event2 = new EventArrayMsgpackIntKeys();
        private byte[] ProtoSerializedData;
        private byte[] JilSerializedData;
        private byte[] GProtoSerialziedData;
        private byte[] MsgPackData;
        private byte[] MsgPackDataIntKeys;
        private byte[] AvroData;
        private byte[] NewtonsoftData;
        private byte[] Utf8JsonData;
        private byte[] SpreadsData;
        private byte[] SystemTextData;
        private byte[] zeroFormatterData;
        private byte[] spanJsonData;
        private byte[] bondData;


        [GlobalSetup]
        public void CreateTestData()
        {
            _events.Events = new TestEvent[20_000];
            _event2.Events = new TestEvent[20_000];
            RuntimeTypeModel.Default.Add(typeof(DateTimeOffset), false).SetSurrogate(typeof(DateTimeOffsetSurrogate));
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            for (int i = 0; i < 20_000; i++)
            {
                var @event = new TestEvent(Guid.NewGuid(), RandomString(5), random.NextDouble(), random.NextDouble(),
                    random.NextDouble(), DateTimeOffset.Now);
                _events.Events[i] = @event;
                _event2.Events[i] = @event;
            }

            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, _events);
                ProtoSerializedData = stream.ToArray();
            }

            JilSerializedData = Encoding.UTF8.GetBytes(Jil.JSON.Serialize(_events));

            var gProtoData = new Testevents.Protobuf.TestEventArray();
            gProtoData.Data.AddRange(_events.Events.Select(e => new Testevents.Protobuf.TestEvent
            {
                Id = ByteString.CopyFrom(e.Id.ToByteArray()),
                Name = e.Name,
                X = e.X,
                Y = e.Y,
                Z = e.Z,
                TimeStamp = Timestamp.FromDateTimeOffset(e.TimeStamp)
            }));
            GProtoSerialziedData = gProtoData.ToByteArray();

            MsgPackData = MessagePackSerializer.Serialize(_events);
            MsgPackDataIntKeys = MessagePackSerializer.Serialize(_event2);

            var newtonJsonString = JsonConvert.SerializeObject(_events);
            NewtonsoftData = Encoding.UTF8.GetBytes(newtonJsonString);

            var avroEvent = new Avro.TestEventArray()
            {
                events = _events.Events.Select(e => new Avro.TestEvent
                {
                    id = e.Id.ToByteArray(),
                    name = e.Name,
                    timeStamp = e.TimeStamp.ToUnixTimeMilliseconds(),
                    x = e.X,
                    y = e.Y,
                    z = e.Z
                }).ToArray()
            };

            using (var stream = new MemoryStream())
            {
                avroSerializer.Write(avroEvent, new BinaryEncoder(stream));
                AvroData = stream.ToArray();
            }

            Utf8JsonData = Utf8Json.JsonSerializer.Serialize(_events, StandardResolver.CamelCase);
            SpreadsData = Spreads.Serialization.Utf8Json.JsonSerializer.Serialize(_events,
                Spreads.Serialization.Utf8Json.Resolvers.StandardResolver.CamelCase);
            SystemTextData = System.Text.Json.Serialization.JsonSerializer.ToUtf8Bytes(_events);
            spanJsonData = SpanJson.JsonSerializer.Generic.Utf8.Serialize<TestEventArray, ExcludeNullsCamelCaseResolver<byte>>(_events);
            zeroFormatterData = ZeroFormatterSerializer.Serialize(_events);

            var output = new OutputBuffer();
            var writer = new CompactBinaryWriter<OutputBuffer>(output);

            var bondEvent = new Bond.TestEventArray
            {
                events = _events.Events.Select(e => new Bond.TestEvent
                {
                    id = e.Id.ToByteArray(),
                    name = e.Name,
                    x = e.X,
                    y = e.Y,
                    z = e.Z,
                    date = e.TimeStamp.ToUnixTimeMilliseconds()
                }).ToList()
            };
            Bond.Serialize.To(writer, bondEvent);
            bondData = output.Data.ToArray();

            Console.WriteLine($"Size of zeroformatter object {zeroFormatterData.Length}");
            Console.WriteLine($"Size of protobuf-net object {ProtoSerializedData.Length}");
            Console.WriteLine($"Size of gproto object {GProtoSerialziedData.Length}");
            Console.WriteLine($"Size of bond object {bondData.Length}");
            Console.WriteLine($"Size of msgPack object {MsgPackData.Length}");
            Console.WriteLine($"Size of msgPack object with int keys {MsgPackDataIntKeys.Length}");
            Console.WriteLine($"Size of avro data {AvroData.Length}");
            Console.WriteLine($"Size of newtonsoft object {NewtonsoftData.Length}");
            Console.WriteLine($"Size of utf8json object {Utf8JsonData.Length}");
            Console.WriteLine($"Size of jil object {JilSerializedData.Length}");
        }

        [Benchmark]
        public byte[] SerializeJil()
        {
            var res = Encoding.UTF8.GetBytes(Jil.JSON.Serialize(_events));
            return res;
        }

        [Benchmark]
        public byte[] SerializeProtobufNet()
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, _events);
                var data = stream.ToArray();
                return data;
            }
        }

        [Benchmark]
        public byte[] SerializeAvro()
        {
            var avroEvent = new Avro.TestEventArray()
            {
                events = _events.Events.Select(e => new Avro.TestEvent
                {
                    id = e.Id.ToByteArray(),
                    name = e.Name,
                    timeStamp = e.TimeStamp.ToUnixTimeMilliseconds(),
                    x = e.X,
                    y = e.Y,
                    z = e.Z
                }).ToArray()
            };
            using (var stream = new MemoryStream())
            {
                avroSerializer.Write(avroEvent, new BinaryEncoder(stream));
                var result = stream.ToArray();
                return result;
            }
        }

        [Benchmark]
        public byte[] SerializeGProto()
        {
            var gProtoData = new Testevents.Protobuf.TestEventArray();
            gProtoData.Data.AddRange(_events.Events.Select(e => new Testevents.Protobuf.TestEvent
            {
                Id = ByteString.CopyFrom(e.Id.ToByteArray()),
                Name = e.Name,
                X = e.X,
                Y = e.Y,
                Z = e.Z,
                TimeStamp = Timestamp.FromDateTimeOffset(e.TimeStamp)
            }));
            var result = gProtoData.ToByteArray();
            return result;
        }

        [Benchmark]
        public byte[] SerializeMsgPack()
        {
            return MessagePackSerializer.Serialize(_events);
        }

        [Benchmark]
        public byte[] SerializeMsgPackIntKey()
        {
            return MessagePackSerializer.Serialize(_event2);
        }

        [Benchmark]
        public byte[] SerializeNewtonsoft()
        {
            var newtonJsonString = JsonConvert.SerializeObject(_events);
            return Encoding.UTF8.GetBytes(newtonJsonString);
        }

        [Benchmark]
        public byte[] SerializeUtf8Json()
        {
            return Utf8Json.JsonSerializer.Serialize(_events, StandardResolver.CamelCase);
        }

        [Benchmark]
        public byte[] SerializeSpanJson()
        {
            return SpanJson.JsonSerializer.Generic.Utf8.Serialize<TestEventArray, ExcludeNullsCamelCaseResolver<byte>>(_events);
        }

        [Benchmark]
        public byte[] SerializeSystemText()
        {
            return System.Text.Json.Serialization.JsonSerializer.ToUtf8Bytes(_events);
        }

        [Benchmark]
        public byte[] SerializeSpreads()
        {
            return Spreads.Serialization.Utf8Json.JsonSerializer.Serialize(_events,
                Spreads.Serialization.Utf8Json.Resolvers.StandardResolver.CamelCase);
        }

        [Benchmark]
        public byte[] SerializeZeroFormatter()
        {
            var result = ZeroFormatterSerializer.Serialize(_events);
            return result;
        }

        [Benchmark]
        public byte[] SerializeBond()
        {
            var output = new OutputBuffer();
            var writer = new CompactBinaryWriter<OutputBuffer>(output);

            var bondEvent = new Bond.TestEventArray
            {
                events = _events.Events.Select(e => new Bond.TestEvent
                {
                    id = e.Id.ToByteArray(),
                    name = e.Name,
                    x = e.X,
                    y = e.Y,
                    z = e.Z,
                    date = e.TimeStamp.ToUnixTimeMilliseconds()
                }).ToList()
            };
            Bond.Serialize.To(writer, bondEvent);
            var result = output.Data.ToArray();
            return result;
        }

        [Benchmark]
        public TestEventArray DeserializeJil()
        {
            var result = JSON.Deserialize<TestEventArray>(Encoding.UTF8.GetString(JilSerializedData));
            return result;
        }

        [Benchmark]
        public TestEventArray DeserializeProtoNet()
        {
            using (var stream = new MemoryStream(ProtoSerializedData))
            {
                return Serializer.Deserialize<TestEventArray>(stream);
            }
        }

        [Benchmark]
        public TestEventArray DeserializeGProto()
        {
            var result = Testevents.Protobuf.TestEventArray.Parser.ParseFrom(GProtoSerialziedData);
            var actual = new TestEventArray
            {
                Events = result.Data.Select(e => new TestEvent(new Guid(e.Id.ToByteArray()), e.Name, e.X, e.Y, e.Z,
                    e.TimeStamp.ToDateTimeOffset())).ToArray()
            };
            return actual;
        }

        [Benchmark]
        public TestEventArray DeserializeMsgPack()
        {
            var result = MessagePackSerializer.Deserialize<TestEventArray>(MsgPackData);
            return result;
        }

        [Benchmark]
        public EventArrayMsgpackIntKeys DeserializeMsgPackIntKeys()
        {
            var result = MessagePackSerializer.Deserialize<EventArrayMsgpackIntKeys>(MsgPackDataIntKeys);
            return result;
        }

        [Benchmark]
        public TestEventArray DeserializeAvro()
        {

            var avroEvent = new Avro.TestEventArray();
            using (var stream = new MemoryStream(AvroData))
                {
                    _avroDeserializer.Read(avroEvent, new BinaryDecoder(stream));
                    var actualEvent = new TestEventArray()
                    {
                        Events = avroEvent.events.Select(e => new TestEvent
                        {
                            Id = new Guid(e.id),
                            Name = e.name,
                            X = e.x,
                            Y = e.y,
                            Z = e.z,
                            TimeStamp = DateTimeOffset.FromUnixTimeMilliseconds(e.timeStamp)
                        }).ToArray()
                    };
                    return actualEvent;
                }
        }

        [Benchmark]
        public TestEventArray DeserializeNewtonsoft()
        {
            var result = JsonConvert.DeserializeObject<TestEventArray>(Encoding.UTF8.GetString(NewtonsoftData));
            return result;
        }

        [Benchmark]
        public TestEventArray DeserializeUtf8Json()
        {
            var result = Utf8Json.JsonSerializer.Deserialize<TestEventArray>(Utf8JsonData, StandardResolver.CamelCase);
            return result;
        }

        [Benchmark]
        public TestEventArray DeserializeSpanJson()
        {
            var result =
                SpanJson.JsonSerializer.Generic.Utf8.Deserialize<TestEventArray, ExcludeNullsCamelCaseResolver<byte>>(
                    spanJsonData);
            return result;
        }

        [Benchmark]
        public TestEventArray DeserializeSystemText()
        {
            return System.Text.Json.Serialization.JsonSerializer.Parse<TestEventArray>(SystemTextData.AsSpan());
        }

        [Benchmark]
        public TestEventArray DeserializeSpreads()
        {
            return Spreads.Serialization.Utf8Json.JsonSerializer.Deserialize<TestEventArray>(SpreadsData,
                Spreads.Serialization.Utf8Json.Resolvers.StandardResolver.CamelCase);
        }

        [Benchmark]
        public TestEventArray DeserializeZeroFormatter()
        {
            var result = ZeroFormatterSerializer.Deserialize<TestEventArray>(zeroFormatterData);
            foreach (var entity in result.Events)
            {
                var id = entity.Id;
                var x = entity.X;
                var y = entity.Y;
                var z = entity.Z;
                var name = entity.Name;
                var timeStamp = entity.TimeStamp;
            }

            return result;
        }

        [Benchmark]
        public TestEventArray DeserializeBond()
        {
            var input = new InputBuffer(bondData);
            var reader = new CompactBinaryReader<InputBuffer>(input);
            var @event = Bond.Deserialize<Bond.TestEventArray>.From(reader);
            var result = new TestEventArray
            {
                Events = @event.events.Select(e => new TestEvent
                {
                    Id = new Guid(e.id),
                    Name = e.name,
                    X = e.x,
                    Y = e.y,
                    Z = e.z,
                    TimeStamp = DateTimeOffset.FromUnixTimeMilliseconds(e.date)
                }).ToArray()
            };
            return result;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}