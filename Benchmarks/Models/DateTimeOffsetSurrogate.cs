using System;
using ProtoBuf;

namespace ProtoEvents
{
    public class DateTimeOffsetSurrogate
    {
        [ProtoMember(1)]
        public DateTime Date { get; set; }

        public static implicit operator DateTimeOffsetSurrogate(DateTimeOffset value)
        {
            return new DateTimeOffsetSurrogate { Date = value.UtcDateTime };
        }

        public static implicit operator DateTimeOffset(DateTimeOffsetSurrogate value)
        {
            return new DateTimeOffset(value.Date, TimeSpan.Zero);
        }

    }
}