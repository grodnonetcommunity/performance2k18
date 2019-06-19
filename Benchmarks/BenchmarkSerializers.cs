using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
using Utf8Json.Resolvers;
using ZeroFormatter;

namespace ProtoEvents
{
    [MemoryDiagnoser]
    public class BenchmarkSerializers
    {
        private static Random random = new Random();
        private static SpecificDatumWriter<Avro.TestEvent> avroSerializer = new SpecificDatumWriter<Avro.TestEvent>(Avro.TestEvent._SCHEMA);
        private static SpecificDatumReader<Avro.TestEvent> _avroDeserializer = new SpecificDatumReader<Avro.TestEvent>(Avro.TestEvent._SCHEMA, Avro.TestEvent._SCHEMA);
        private TestEvent _event = new TestEvent();
        private TestEventMessagePackNoKeys _event2 = new TestEventMessagePackNoKeys();
        private byte[] ProtoSerializedData = null;
        private byte[] JilSerializedData = null;
        private byte[] GProtoSerialziedData = null;
        private byte[] MsgPackData = null;
        private byte[] MsgPackDataIntKeys = null;
        private byte[] AvroData = null;
        private byte[] NewtonsoftData = null;
        private byte[] Utf8JsonData = null;
        private byte[] zeroFormatterData = null;
        private byte[] bondData = null;
        private byte[] SystemTextData = null;
        private byte[] spreadsData = null;


        [GlobalSetup]
        public void CreateTestData()
        {
            RuntimeTypeModel.Default.Add(typeof(DateTimeOffset), false).SetSurrogate(typeof(DateTimeOffsetSurrogate));

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

                var @event = new TestEvent(Guid.NewGuid(), RandomString(5), random.NextDouble(), random.NextDouble(), random.NextDouble(), DateTimeOffset.Now);
                _event  = @event;

                using (var stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, @event);
                    ProtoSerializedData = stream.ToArray();
                }

                var jilJsonString = JSON.Serialize(@event);
                JilSerializedData = Encoding.UTF8.GetBytes(jilJsonString);
                var protoMessage = new Testevents.Protobuf.TestEvent
                {
                    Id = ByteString.CopyFrom(@event.Id.ToByteArray()),
                    Name = @event.Name,
                    X = @event.X,
                    Y = @event.Y,
                    Z = @event.Z,
                    TimeStamp = Timestamp.FromDateTimeOffset(@event.TimeStamp),
                };
                GProtoSerialziedData = SerializeGProto(protoMessage);

                var msgPackResult = MessagePackSerializer.Serialize(@event);
                MsgPackData = msgPackResult;

                var msgPackKeyString = new TestEventMessagePackNoKeys
                {
                    Id = @event.Id,
                    Name = @event.Name,
                    X = @event.X,
                    Y = @event.Y,
                    Z = @event.Z,
                    TimeStamp = @event.TimeStamp
                };
                _event2 = msgPackKeyString;
                var msgPackStringKeysResult = MessagePackSerializer.Serialize(msgPackKeyString);
                MsgPackDataIntKeys = msgPackStringKeysResult;

                var newtonJsonString = JsonConvert.SerializeObject(@event);
                NewtonsoftData = Encoding.UTF8.GetBytes(newtonJsonString);

                var avroEvent = new Avro.TestEvent
                {
                    id = @event.Id.ToByteArray(),
                    name = @event.Name,
                    timeStamp = @event.TimeStamp.ToUnixTimeMilliseconds(),
                    x = @event.X,
                    y = @event.Y,
                    z = @event.Z
                };
                using (var stream = new MemoryStream())
                {
                    avroSerializer.Write(avroEvent, new BinaryEncoder(stream));
                    AvroData = stream.ToArray();
                }

                Utf8JsonData = Utf8Json.JsonSerializer.Serialize(@event, StandardResolver.CamelCase);
                spreadsData = Spreads.Serialization.Utf8Json.JsonSerializer.Serialize(@event,
                    Spreads.Serialization.Utf8Json.Resolvers.StandardResolver.CamelCase);
                zeroFormatterData = ZeroFormatterSerializer.Serialize(@event);
                SystemTextData = System.Text.Json.Serialization.JsonSerializer.ToUtf8Bytes(@event);

                var output = new OutputBuffer();
                var writer = new CompactBinaryWriter<OutputBuffer>(output);
                var bondEvent = new Bond.TestEvent
                {
                    id = @event.Id.ToByteArray(),
                    name = @event.Name,
                    x = @event.X,
                    y = @event.Y,
                    z = @event.Z,
                    date = @event.TimeStamp.ToUnixTimeMilliseconds()
                };
                Bond.Serialize.To(writer, bondEvent);
                bondData = output.Data.ToArray();

            /*Console.WriteLine($"Size of jil object {JilSerializedData.First().Length}");
            Console.WriteLine($"Size of protobuf-net object {ProtoSerializedData.First().Length}");
            Console.WriteLine($"Size of gproto object {GProtoSerialziedData.First().Length}");
            Console.WriteLine($"Size of msgPack object {MsgPackData.First().Length}");
            Console.WriteLine($"Size of msgPack object with int keys {MsgPackDataIntKeys.First().Length}");
            Console.WriteLine($"Size of avro data {AvroData.First().Length}");
            Console.WriteLine($"Size of newtonsoft object {NewtonsoftData.First().Length}");
            Console.WriteLine($"Size of utf8json object {Utf8JsonData.First().Length}");
            Console.WriteLine($"Size of zeroformatter object {zeroFormatterData.First().Length}");*/
        }

        [Benchmark]
        public byte[] SerializeJil()
        {
            return Encoding.UTF8.GetBytes(JSON.Serialize(_event));
        }

        [Benchmark]
        public byte[] SerializeProtobufNet()
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, _event);
                return stream.ToArray();
            }
        }

        [Benchmark]
        public byte[] SerializeAvro()
        {
            byte[] result = null;

            var avroEvent = new Avro.TestEvent
            {
                id = _event.Id.ToByteArray(),
                name = _event.Name,
                timeStamp = _event.TimeStamp.ToUnixTimeMilliseconds(),
                x = _event.X,
                y = _event.Y,
                z = _event.Z
            };
            using (var stream = new MemoryStream())
            {
                avroSerializer.Write(avroEvent, new BinaryEncoder(stream));
                return stream.ToArray();
            }
        }

        [Benchmark]
        public byte[] SerializeSystemText()
        {
            var result = System.Text.Json.Serialization.JsonSerializer.ToUtf8Bytes(_event);
            return result;
        }

        [Benchmark]
        public byte[] SerializeGProto()
        {
            var data = new Testevents.Protobuf.TestEvent
            {
                Id = ByteString.CopyFrom(_event.Id.ToByteArray()),
                Name = _event.Name,
                X = _event.X,
                Y = _event.Y,
                Z = _event.Z,
                TimeStamp = Timestamp.FromDateTimeOffset(_event.TimeStamp)
            };
            return data.ToByteArray();
        }

        [Benchmark]
        public byte[] SerializeMsgPack()
        {
            var result = MessagePackSerializer.Serialize(_event);
            return result;
        }

        [Benchmark]
        public byte[] SerializeMsgPackIntKey()
        {
            var result = MessagePackSerializer.Serialize(_event2);
            return result;
        }

        [Benchmark]
        public byte[] SerializeNewtonsoft()
        {
            var result = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_event));
            return result;
        }

        [Benchmark]
        public byte[] SerializeUtf8Json()
        {
            var result = Utf8Json.JsonSerializer.Serialize(_event, StandardResolver.CamelCase);
            return result;
        }

        [Benchmark]
        public byte[] SerializeSpreads()
        {
            var result = Spreads.Serialization.Utf8Json.JsonSerializer.Serialize(_event,
                Spreads.Serialization.Utf8Json.Resolvers.StandardResolver.CamelCase);
            return result;
        }

        [Benchmark]
        public byte[] SerializeZeroFormatter()
        {
            var result = ZeroFormatterSerializer.Serialize(_event);
            return result;
        }

        [Benchmark]
        public byte[] SerializeBond()
        {
            byte[] result = null;
            var output = new OutputBuffer();
            var writer = new CompactBinaryWriter<OutputBuffer>(output);
            var bondEvent = new Bond.TestEvent
            {
                id = _event.Id.ToByteArray(),
                name = _event.Name,
                x = _event.X,
                y = _event.Y,
                z = _event.Z,
                date = _event.TimeStamp.ToUnixTimeMilliseconds()
            };
            Bond.Serialize.To(writer, bondEvent);
            result = output.Data.ToArray();

            return result;
        }

        [Benchmark]
        public TestEvent DeserializeJil()
        {
            return JSON.Deserialize<TestEvent>(Encoding.UTF8.GetString(JilSerializedData));
        }

        [Benchmark]
        public TestEvent DeserializeProtoNet()
        {
            using (var stream = new MemoryStream(ProtoSerializedData))
            {
                return Serializer.Deserialize<TestEvent>(stream);
            }
        }

        [Benchmark]
        public TestEvent DeserializeGProto()
        {
            var result = Testevents.Protobuf.TestEvent.Parser.ParseFrom(GProtoSerialziedData);
            var actual = new TestEvent(new Guid(result.Id.ToByteArray()), result.Name, result.X, result.Y, result.Z,
                result.TimeStamp.ToDateTimeOffset());
            return actual;
        }

        [Benchmark]
        public TestEvent DeserializeMsgPack()
        {
            var result = MessagePackSerializer.Deserialize<TestEvent>(MsgPackData);
            return result;
        }

        [Benchmark]
        public TestEventMessagePackNoKeys DeserializeMsgPackIntKeys()
        {
            var result = MessagePackSerializer.Deserialize<TestEventMessagePackNoKeys>(MsgPackDataIntKeys);
            return result;
        }

        [Benchmark]
        public TestEvent DeserializeAvro()
        {
            using (var stream = new MemoryStream(AvroData))
            {
                var avroEvent = _avroDeserializer.Read(new Avro.TestEvent(), new BinaryDecoder(stream));
                var result = new TestEvent
                {
                    Id = new Guid(avroEvent.id),
                    Name = avroEvent.name,
                    X = avroEvent.x,
                    Y = avroEvent.y,
                    Z = avroEvent.z,
                    TimeStamp = DateTimeOffset.FromUnixTimeMilliseconds(avroEvent.timeStamp)
                };
                return result;
            }
        }

        [Benchmark]
        public TestEvent DeserializeNewtonsoft()
        {
            var result = JsonConvert.DeserializeObject<TestEvent>(Encoding.UTF8.GetString(NewtonsoftData));
            return result;
        }

        [Benchmark]
        public TestEvent DeserializeSystemText()
        {
            var result = System.Text.Json.Serialization.JsonSerializer.Parse<TestEvent>(SystemTextData.AsSpan());
            return result;
        }

        [Benchmark]
        public TestEvent DeserializeSpreads()
        {
            var result = Spreads.Serialization.Utf8Json.JsonSerializer.Deserialize<TestEvent>(spreadsData,
                Spreads.Serialization.Utf8Json.Resolvers.StandardResolver.CamelCase);
            return result;
        }

        [Benchmark]
        public TestEvent DeserializeUtf8Json()
        {
            var result = Utf8Json.JsonSerializer.Deserialize<TestEvent>(Utf8JsonData, StandardResolver.CamelCase);
            return result;
        }

        [Benchmark]
        public TestEvent DeserializeZeroFormatter()
        {
            TestEvent result = null;
            result = ZeroFormatterSerializer.Deserialize<TestEvent>(zeroFormatterData);
            var id = result.Id;
            var x = result.X;
            var y = result.Y;
            var z = result.Z;
            var name = result.Name;
            var timeStamp = result.TimeStamp;

            return result;
        }

        [Benchmark]
        public TestEvent DeserializeBond()
        {
            TestEvent result = null;
            var input = new InputBuffer(bondData);
            var reader = new CompactBinaryReader<InputBuffer>(input);
            var @event = Bond.Deserialize<Bond.TestEvent>.From(reader);
            result = new TestEvent
            {
                Id = new Guid(@event.id),
                Name = @event.name,
                X = @event.x,
                Y = @event.y,
                Z = @event.z,
                TimeStamp = DateTimeOffset.FromUnixTimeMilliseconds(@event.date)
            };

            return result;
        }

        private static byte[] SerializeGProto(Testevents.Protobuf.TestEvent message)
        {
            using (var stream = new MemoryStream())
            {
                message.WriteTo(stream);
                return stream.ToArray();
            }
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}