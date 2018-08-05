using System;

namespace DeltaJsonDeserialization.Server
{
    public class SampleModel
    {
        public bool Bool { get; set; }

        public byte Byte { get; set; }

        public byte[] Bytes { get; set; }

        public DateTime DateTime { get; set; }

        public DateTime? NullableDateTime { get; set; }

        public DateTimeOffset DateTimeOffset { get; set; }

        public DateTimeOffset? NullableDateTimeOffset { get; set; }

        public double Double { get; set; }

        public double? NullableDouble { get; set; }

        public int Int { get; set; }

        public int? NullableInt { get; set; }

        public string String { get; set; }

        public DayOfWeek Enum { get; set; }
    }
}