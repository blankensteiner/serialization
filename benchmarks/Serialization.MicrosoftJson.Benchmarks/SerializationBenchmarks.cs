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
    public class SerializationBenchmarks
    {
        private readonly ISerializer _serializer;
        private readonly ISerializer<ClosedClass> _serializerOfT;
        private readonly OpenClass _openClass;
        private readonly ClosedClass _closedClass;

        public SerializationBenchmarks()
        {
            var builder = new SerializerBuilder(new System.Text.Json.JsonSerializerOptions());
            builder.Type<ClosedClass>();
            _serializer = builder.Build();
            _serializerOfT = _serializer.GetSerializerFor<ClosedClass>();

            _openClass = new OpenClass
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
                DateTime = DateTime.UtcNow,
                DateTimeOffset = DateTimeOffset.UtcNow,
                Guid = Guid.NewGuid(),
                String = "Test"
            };

            _closedClass = new ClosedClass(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, Guid.NewGuid(), DateTime.UtcNow, DateTimeOffset.UtcNow, "Test", true);
        }

        [Benchmark]
        public byte[] System_Text_Json_JsonSerializer() => System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(_openClass);

        [Benchmark]
        public byte[] Serialization_MicrosoftJson_Serializer() => _serializer.Serialize(_closedClass);

        [Benchmark]
        public byte[] Serialization_MicrosoftJson_Serializer_Of_T() => _serializerOfT.Serialize(_closedClass);
    }
}
