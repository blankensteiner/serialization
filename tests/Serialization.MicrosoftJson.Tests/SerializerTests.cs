namespace Serialization.MicrosoftJson.Tests
{
    using FluentAssertions;
    using Serialization.Abstractions;
    using System;
    using Xunit;

    public class SerializerTests
    {
        private readonly ISerializer _serializer;

        public SerializerTests()
        {
            var builder = new SerializerBuilder(new System.Text.Json.JsonSerializerOptions());
            builder.Type<ClosedClass>();
            _serializer = builder.Build();
        }

        [Fact]
        public void SerializeToByteArray()
        {
            // Arrange
            var expected = new ClosedClass(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, Guid.NewGuid(), DateTime.UtcNow, DateTimeOffset.UtcNow, "Test", true);

            // Act
            var bytes = _serializer.Serialize(expected);

            // Assert
            using var jsonDocument = System.Text.Json.JsonDocument.Parse(bytes);
            var actual = jsonDocument.RootElement;
            actual.GetProperty("Boolean").GetBoolean().Should().Be(expected.Boolean);
            actual.GetProperty("Byte").GetByte().Should().Be(expected.Byte);
            actual.GetProperty("Sbyte").GetSByte().Should().Be(expected.Sbyte);
            actual.GetProperty("Short").GetInt16().Should().Be(expected.Short);
            actual.GetProperty("Ushort").GetUInt16().Should().Be(expected.Ushort);
            actual.GetProperty("Int").GetInt32().Should().Be(expected.Int);
            actual.GetProperty("Uint").GetUInt32().Should().Be(expected.Uint);
            actual.GetProperty("Long").GetInt64().Should().Be(expected.Long);
            actual.GetProperty("Ulong").GetUInt64().Should().Be(expected.Ulong);
            actual.GetProperty("Float").GetSingle().Should().Be(expected.Float);
            actual.GetProperty("Double").GetDouble().Should().Be(expected.Double);
            actual.GetProperty("Decimal").GetDecimal().Should().Be(expected.Decimal);
            actual.GetProperty("DateTime").GetDateTime().Should().Be(expected.DateTime);
            actual.GetProperty("DateTimeOffset").GetDateTimeOffset().Should().Be(expected.DateTimeOffSet);
            actual.GetProperty("Guid").GetGuid().Should().Be(expected.Guid);
            actual.GetProperty("String").GetString().Should().Be(expected.String);
        }

        [Fact]
        public void DeserializeFromByteArray()
        {
            // Arrange
            var expected = new OpenClass
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

            var bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(expected);

            // Act
            var actual = _serializer.Deserialize<ClosedClass>(bytes);

            // Assert
            actual.Boolean.Should().Be(expected.Boolean);
            actual.Byte.Should().Be(expected.Byte);
            actual.Sbyte.Should().Be(expected.Sbyte);
            actual.Short.Should().Be(expected.Short);
            actual.Ushort.Should().Be(expected.Ushort);
            actual.Int.Should().Be(expected.Int);
            actual.Uint.Should().Be(expected.Uint);
            actual.Long.Should().Be(expected.Long);
            actual.Ulong.Should().Be(expected.Ulong);
            actual.Float.Should().Be(expected.Float);
            actual.Double.Should().Be(expected.Double);
            actual.Decimal.Should().Be(expected.Decimal);
            actual.DateTime.Should().Be(expected.DateTime);
            actual.DateTimeOffSet.Should().Be(expected.DateTimeOffset);
            actual.Guid.Should().Be(expected.Guid);
            actual.String.Should().Be(expected.String);
        }
    }
}
