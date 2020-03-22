namespace Serialization.MicrosoftJson.Tests
{
    using Serialization.Abstractions;
    using System;

    public sealed class Fixture
    {
        public Fixture()
        {
            var builder = new SerializerBuilder();
            builder.Type<ClosedClass<Int8Enum>>();
            builder.Type<ClosedClass<UInt8Enum>>();
            builder.Type<ClosedClass<Int16Enum>>();
            builder.Type<ClosedClass<UInt16Enum>>();
            builder.Type<ClosedClass<Int32Enum>>();
            builder.Type<ClosedClass<UInt32Enum>>();
            builder.Type<ClosedClass<Int64Enum>>();
            builder.Type<ClosedClass<UInt64Enum>>();
            builder.Type<ClosedClass<bool>>();
            builder.Type<ClosedClass<byte>>();
            builder.Type<ClosedClass<sbyte>>();
            builder.Type<ClosedClass<short>>();
            builder.Type<ClosedClass<ushort>>();
            builder.Type<ClosedClass<int>>();
            builder.Type<ClosedClass<uint>>();
            builder.Type<ClosedClass<long>>();
            builder.Type<ClosedClass<ulong>>();
            builder.Type<ClosedClass<float>>();
            builder.Type<ClosedClass<double>>();
            builder.Type<ClosedClass<decimal>>();
            builder.Type<ClosedClass<string>>();
            builder.Type<ClosedClass<Guid>>();
            builder.Type<ClosedClass<DateTime>>();
            builder.Type<ClosedClass<DateTimeOffset>>();
            builder.Type<ClosedClass<bool?>>();
            builder.Type<ClosedClass<byte?>>();
            builder.Type<ClosedClass<sbyte?>>();
            builder.Type<ClosedClass<short?>>();
            builder.Type<ClosedClass<ushort?>>();
            builder.Type<ClosedClass<int?>>();
            builder.Type<ClosedClass<uint?>>();
            builder.Type<ClosedClass<long?>>();
            builder.Type<ClosedClass<ulong?>>();
            builder.Type<ClosedClass<float?>>();
            builder.Type<ClosedClass<double?>>();
            builder.Type<ClosedClass<decimal?>>();
            builder.Type<ClosedClass<Guid?>>();
            builder.Type<ClosedClass<DateTime?>>();
            builder.Type<ClosedClass<DateTimeOffset?>>();
            Serializer = builder.Build();
        }

        public ISerializer Serializer { get; }
    }
}
