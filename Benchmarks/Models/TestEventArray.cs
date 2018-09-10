using System.Collections.Generic;
using Jil;
using MessagePack;
using ProtoBuf;
using ZeroFormatter;

namespace ProtoEvents
{
    [ProtoContract]
    [MessagePackObject(true)]
    [ZeroFormattable]
    public class TestEventArray
    {
        [ProtoMember(1, Name = "events")]
        [Index(0)]
        [JilDirective("events")]
        public virtual TestEvent[] Events { get; set; }
    }
}