using System;
using Jil;
using MessagePack;
using ProtoBuf;
using ZeroFormatter;

namespace ProtoEvents
{
    [ProtoContract]
    [MessagePackObject(true)]
    [ZeroFormattable]
    public class TestEvent
    {
        public TestEvent()
        {
        }

        public TestEvent(Guid id, string name, double x, double y, double z, DateTimeOffset dateTimeOffset)
        {
            Id = id;
            Name = name;
            X = x;
            Y = y;
            Z = z;
            TimeStamp = dateTimeOffset;
        }

        [ProtoMember(1, Name = "id")]
        [Index(0)]
        [JilDirective("id")]
        public virtual Guid Id { get; set; }

        [ProtoMember(2, Name = "name")]
        [Index(1)]
        [JilDirective("name")]
        public virtual string Name { get; set; }

        [ProtoMember(3, Name = "x")]
        [Index(2)]
        [JilDirective("x")]
        public virtual double X { get; set; }

        [ProtoMember(4, Name = "y")]
        [Index(3)]
        [JilDirective("y")]
        public virtual double Y { get; set; }

        [ProtoMember(5, Name = "z")]
        [Index(4)]
        [JilDirective("z")]
        public virtual double Z { get; set; }

        [ProtoMember(6, Name = "timeStamp")]
        [Index(5)]
        [JilDirective("timeStamp")]
        public virtual DateTimeOffset TimeStamp { get; set; }

    }
}