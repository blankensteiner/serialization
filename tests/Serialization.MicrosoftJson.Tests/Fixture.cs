namespace Serialization.MicrosoftJson.Tests
{
    using Serialization.Abstractions;
    using System;

    public sealed class Fixture
    {
        public Fixture()
        {
            var builder = new SerializerBuilder();
            builder.AddType<ClosedClass<Int8Enum>>();
            builder.AddType<ClosedClass<UInt8Enum>>();
            builder.AddType<ClosedClass<Int16Enum>>();
            builder.AddType<ClosedClass<UInt16Enum>>();
            builder.AddType<ClosedClass<Int32Enum>>();
            builder.AddType<ClosedClass<UInt32Enum>>();
            builder.AddType<ClosedClass<Int64Enum>>();
            builder.AddType<ClosedClass<UInt64Enum>>();
            builder.AddType<ClosedClass<bool>>();
            builder.AddType<ClosedClass<byte>>();
            builder.AddType<ClosedClass<sbyte>>();
            builder.AddType<ClosedClass<short>>();
            builder.AddType<ClosedClass<ushort>>();
            builder.AddType<ClosedClass<int>>();
            builder.AddType<ClosedClass<uint>>();
            builder.AddType<ClosedClass<long>>();
            builder.AddType<ClosedClass<ulong>>();
            builder.AddType<ClosedClass<float>>();
            builder.AddType<ClosedClass<double>>();
            builder.AddType<ClosedClass<decimal>>();
            builder.AddType<ClosedClass<string>>();
            builder.AddType<ClosedClass<Guid>>();
            builder.AddType<ClosedClass<DateTime>>();
            builder.AddType<ClosedClass<DateTimeOffset>>();
            builder.AddType<ClosedClass<Int8Enum?>>();
            builder.AddType<ClosedClass<UInt8Enum?>>();
            builder.AddType<ClosedClass<Int16Enum?>>();
            builder.AddType<ClosedClass<UInt16Enum?>>();
            builder.AddType<ClosedClass<Int32Enum?>>();
            builder.AddType<ClosedClass<UInt32Enum?>>();
            builder.AddType<ClosedClass<Int64Enum?>>();
            builder.AddType<ClosedClass<UInt64Enum?>>();
            builder.AddType<ClosedClass<bool?>>();
            builder.AddType<ClosedClass<byte?>>();
            builder.AddType<ClosedClass<sbyte?>>();
            builder.AddType<ClosedClass<short?>>();
            builder.AddType<ClosedClass<ushort?>>();
            builder.AddType<ClosedClass<int?>>();
            builder.AddType<ClosedClass<uint?>>();
            builder.AddType<ClosedClass<long?>>();
            builder.AddType<ClosedClass<ulong?>>();
            builder.AddType<ClosedClass<float?>>();
            builder.AddType<ClosedClass<double?>>();
            builder.AddType<ClosedClass<decimal?>>();
            builder.AddType<ClosedClass<Guid?>>();
            builder.AddType<ClosedClass<DateTime?>>();
            builder.AddType<ClosedClass<DateTimeOffset?>>();
            //builder.AddType<ClosedClass<ClosedClass<int>>>();
            Serializer = builder.Build();
        }

        public ISerializer Serializer { get; }
    }
}
