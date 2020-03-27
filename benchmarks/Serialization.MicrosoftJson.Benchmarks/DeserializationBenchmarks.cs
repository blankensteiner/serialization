namespace Serialization.MicrosoftJson.Benchmarks
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Order;
    using Serialization.Abstractions;
    using System;

    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    [RankColumn]
    public class DeserializationBenchmarks
    {
        private readonly byte[] _bytes;
        private readonly Guid _guid;
        private readonly DateTime _dateTime;
        private readonly DateTimeOffset _dateTimeOffset;
        private readonly ISerializer _serializer;
        private readonly ISerializer<ClosedClass> _serializerOfT;

        public DeserializationBenchmarks()
        {
            var builder = new SerializerBuilder();
            builder.AddType<ClosedClass>();
            _serializer = builder.Build();
            _serializerOfT = _serializer.GetSerializerFor<ClosedClass>();

            _guid = Guid.NewGuid();
            _dateTime = DateTime.UtcNow;
            _dateTimeOffset = DateTimeOffset.UtcNow;

            var openClass = new OpenClass
            {
                Boolean = true,
                Byte = 1,
                Sbyte = 2,
                Short = 3,
                Ushort = 4,
                Int = 5,
                Uint = 6,
                Long = 7,
                Ulong = 8,
                Float = 9,
                Double = 10,
                Decimal = 11,
                DateTime = _dateTime,
                DateTimeOffset = _dateTimeOffset,
                Guid = _guid,
                String = "Test"
            };
            _bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(openClass);
        }

        [Benchmark]
        public OpenClass System_Text_Json_JsonSerializer() => System.Text.Json.JsonSerializer.Deserialize<OpenClass>(_bytes);

        [Benchmark]
        public ClosedClass Serialization_MicrosoftJson_Serializer() => _serializer.Deserialize<ClosedClass>(_bytes);

        [Benchmark]
        public ClosedClass Serialization_MicrosoftJson_SerializerOfT() => _serializerOfT.Deserialize(_bytes);
    }
}
