namespace Serialization.MicrosoftJson.Tests
{
    using System;

    public sealed class ClosedClass
    {
        private readonly byte _byte;
        private readonly sbyte _sbyte;
        private readonly short _short;
        private readonly ushort _ushort;
        private readonly int _int;
        private readonly uint _uint;
        private readonly long _long;
        private readonly ulong _ulong;
        private readonly float _float;
        private readonly double _double;
        private readonly decimal _decimal;
        private readonly Guid _guid;
        private readonly DateTime _dateTime;
        private readonly DateTimeOffset _dateTimeOffset;
        private readonly string _string;
        private readonly bool _boolean;

        public ClosedClass(
            byte byte1,
            sbyte sbyte1,
            short short1,
            ushort ushort1,
            int int1,
            uint uint1,
            long long1,
            ulong ulong1,
            float float1,
            double double1,
            decimal decimal1,
            Guid guid1,
            DateTime dateTime1,
            DateTimeOffset dateTimeOffset1,
            string string1,
            bool boolean1)
        {
            _byte = byte1;
            _sbyte = sbyte1;
            _short = short1;
            _ushort = ushort1;
            _int = int1;
            _uint = uint1;
            _long = long1;
            _ulong = ulong1;
            _float = float1;
            _double = double1;
            _decimal = decimal1;
            _guid = guid1;
            _dateTime = dateTime1;
            _dateTimeOffset = dateTimeOffset1;
            _string = string1;
            _boolean = boolean1;
        }

        public byte Byte => _byte;
        public sbyte Sbyte => _sbyte;
        public short Short => _short;
        public ushort Ushort => _ushort;
        public int Int => _int;
        public uint Uint => _uint;
        public long Long => _long;
        public ulong Ulong => _ulong;
        public float Float => _float;
        public double Double => _double;
        public decimal Decimal => _decimal;
        public Guid Guid => _guid;
        public DateTime DateTime => _dateTime;
        public DateTimeOffset DateTimeOffSet => _dateTimeOffset;
        public string String => _string;
        public bool Boolean => _boolean;
    }
}
