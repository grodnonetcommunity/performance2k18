using System;
using MessagePack;

namespace ProtoEvents
{
    [MessagePackObject]
    public class TestEventMessagePackNoKeys
    {
        [Key(0)]
        public Guid Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public double X { get; set; }
        [Key(3)]
        public double Y { get; set; }
        [Key(4)]
        public double Z { get; set; }
        [Key(5)]
        public DateTimeOffset TimeStamp { get; set; }
    }
}