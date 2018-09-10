using MessagePack;

namespace ProtoEvents
{
    [MessagePackObject]
    public class EventArrayMsgpackIntKeys
    {
        [Key(0)]
        public TestEvent[] Events { get; set; }
    }
}