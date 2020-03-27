namespace Serialization.MicrosoftJson.Tests
{
    using FluentAssertions;
    using Serialization.Abstractions;
    using System;
    using System.Text;
    using System.Text.Json;
    using Xunit;
    using static Serialization.MicrosoftJson.Tests.JsonHelpers;

    public class SerializerTests : IClassFixture<Fixture>
    {
        private readonly ISerializer _serializer;

        public SerializerTests(Fixture fixture) => _serializer = fixture.Serializer;

        private ClosedClass<TValue> Deserialize<TValue>(OpenClass<TValue> openClass)
            => Deserialize<TValue>(JsonSerializer.SerializeToUtf8Bytes(openClass));

        private ClosedClass<TValue> Deserialize<TValue>(string json)
            => Deserialize<TValue>(Encoding.UTF8.GetBytes(json));

        private ClosedClass<TValue> Deserialize<TValue>(byte[] bytes)
            => _serializer.Deserialize<ClosedClass<TValue>>(bytes);

        [Fact]
        public void Serialize_WhenValueIsBoolean_ShouldSerializeValue()
        {

            var expected = new ClosedClass<bool>(true);             // Arrange
            var bytes = _serializer.Serialize(expected);            // Act
            GetBoolean(bytes, "Value").Should().Be(expected.Value); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsByte_ShouldSerializeValue()
        {

            var expected = new ClosedClass<byte>(byte.MaxValue); // Arrange
            var bytes = _serializer.Serialize(expected);         // Act
            GetInt8(bytes, "Value").Should().Be(expected.Value); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsSByte_ShouldSerializeValue()
        {

            var expected = new ClosedClass<sbyte>(sbyte.MaxValue); // Arrange
            var bytes = _serializer.Serialize(expected);           // Act
            GetUInt8(bytes, "Value").Should().Be(expected.Value);  // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsInt16_ShouldSerializeValue()
        {

            var expected = new ClosedClass<short>(short.MaxValue); // Arrange
            var bytes = _serializer.Serialize(expected);           // Act
            GetInt16(bytes, "Value").Should().Be(expected.Value);  // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsUInt16_ShouldSerializeValue()
        {

            var expected = new ClosedClass<ushort>(ushort.MaxValue); // Arrange
            var bytes = _serializer.Serialize(expected);             // Act
            GetUInt16(bytes, "Value").Should().Be(expected.Value);   // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsInt32_ShouldSerializeValue()
        {

            var expected = new ClosedClass<int>(int.MaxValue);    // Arrange
            var bytes = _serializer.Serialize(expected);          // Act
            GetInt32(bytes, "Value").Should().Be(expected.Value); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsUInt32_ShouldSerializeValue()
        {
            
            var expected = new ClosedClass<uint>(uint.MaxValue);   // Arrange
            var bytes = _serializer.Serialize(expected);           // Act
            GetUInt32(bytes, "Value").Should().Be(expected.Value); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsInt64_ShouldSerializeValue()
        {

            var expected = new ClosedClass<long>(long.MaxValue);  // Arrange
            var bytes = _serializer.Serialize(expected);          // Act
            GetInt64(bytes, "Value").Should().Be(expected.Value); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsUInt64_ShouldSerializeValue()
        {

            var expected = new ClosedClass<ulong>(ulong.MaxValue); // Arrange
            var bytes = _serializer.Serialize(expected);           // Act
            GetUInt64(bytes, "Value").Should().Be(expected.Value); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsFloat_ShouldSerializeValue()
        {

            var expected = new ClosedClass<float>(float.MaxValue); // Arrange
            var bytes = _serializer.Serialize(expected);           // Act
            GetSingle(bytes, "Value").Should().Be(expected.Value); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsDouble_ShouldSerializeValue()
        {

            var expected = new ClosedClass<double>(double.MaxValue); // Arrange
            var bytes = _serializer.Serialize(expected);             // Act
            GetDouble(bytes, "Value").Should().Be(expected.Value);   // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsDecimal_ShouldSerializeValue()
        {

            var expected = new ClosedClass<decimal>(decimal.MaxValue); // Arrange
            var bytes = _serializer.Serialize(expected);               // Act
            GetDecimal(bytes, "Value").Should().Be(expected.Value);    // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsStringAndNull_ShouldNotSerializeValue()
        {

            var expected = new ClosedClass<string?>(null);     // Arrange
            var bytes = _serializer.Serialize(expected);       // Act
            PropertyExists(bytes, "Value").Should().BeFalse(); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsStringAndSet_ShouldSerializeValue()
        {

            var expected = new ClosedClass<string>("test");        // Arrange
            var bytes = _serializer.Serialize(expected);           // Act
            GetString(bytes, "Value").Should().Be(expected.Value); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsGuid_ShouldSerializeValue()
        {

            var expected = new ClosedClass<Guid>(Guid.NewGuid()); // Arrange
            var bytes = _serializer.Serialize(expected);          // Act
            GetGuid(bytes, "Value").Should().Be(expected.Value);  // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsDateTime_ShouldSerializeValue()
        {

            var expected = new ClosedClass<DateTime>(DateTime.MaxValue); // Arrange
            var bytes = _serializer.Serialize(expected);                 // Act
            GetDateTime(bytes, "Value").Should().Be(expected.Value);     // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsDateTimeOffset_ShouldSerializeValue()
        {

            var expected = new ClosedClass<DateTimeOffset>(DateTimeOffset.MaxValue); // Arrange
            var bytes = _serializer.Serialize(expected);                             // Act
            GetDateTimeOffset(bytes, "Value").Should().Be(expected.Value);           // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsNullableEnumAndSet_ShouldSerializeValue()
        {

            var expected = new ClosedClass<Int8Enum?>(Int8Enum.Max); // Arrange
            var bytes = _serializer.Serialize(expected);             // Act
            GetInt8(bytes, "Value").Should().Be((byte)Int8Enum.Max); // Assert. Use Int8Enum.Max to avoid null ref warning
        }

        [Fact]
        public void Serialize_WhenValueIsNullableEnumAndNull_ShouldNotSerializeValue()
        {

            var expected = new ClosedClass<Int8Enum?>(null);   // Arrange
            var bytes = _serializer.Serialize(expected);       // Act
            PropertyExists(bytes, "Value").Should().BeFalse(); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsNullableAndSet_ShouldSerializeValue()
        {

            var expected = new ClosedClass<int?>(int.MaxValue);   // Arrange
            var bytes = _serializer.Serialize(expected);          // Act
            GetInt32(bytes, "Value").Should().Be(expected.Value); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsNullableAndNull_ShouldNotSerializeValue()
        {

            var expected = new ClosedClass<int?>(null);        // Arrange
            var bytes = _serializer.Serialize(expected);       // Act
            PropertyExists(bytes, "Value").Should().BeFalse(); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsByteEnum_ShouldSerializeValue()
        {
            var expected = new ClosedClass<Int8Enum>(Int8Enum.Max);    // Arrange
            var bytes = _serializer.Serialize(expected);               // Act
            GetInt8(bytes, "Value").Should().Be((byte)expected.Value); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsSByteEnum_ShouldSerializeValue()
        {
            var expected = new ClosedClass<UInt8Enum>(UInt8Enum.Max);    // Arrange
            var bytes = _serializer.Serialize(expected);                 // Act
            GetUInt8(bytes, "Value").Should().Be((sbyte)expected.Value); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsInt16Enum_ShouldSerializeValue()
        {
            var expected = new ClosedClass<Int16Enum>(Int16Enum.Max);    // Arrange
            var bytes = _serializer.Serialize(expected);                 // Act
            GetInt16(bytes, "Value").Should().Be((short)expected.Value); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsUInt16Enum_ShouldSerializeValue()
        {
            var expected = new ClosedClass<UInt16Enum>(UInt16Enum.Max);    // Arrange
            var bytes = _serializer.Serialize(expected);                   // Act
            GetUInt16(bytes, "Value").Should().Be((ushort)expected.Value); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsInt32Enum_ShouldSerializeValue()
        {
            var expected = new ClosedClass<Int32Enum>(Int32Enum.Max);  // Arrange
            var bytes = _serializer.Serialize(expected);               // Act
            GetInt32(bytes, "Value").Should().Be((int)expected.Value); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsUInt32Enum_ShouldSerializeValue()
        {
            var expected = new ClosedClass<UInt32Enum>(UInt32Enum.Max);  // Arrange
            var bytes = _serializer.Serialize(expected);                 // Act
            GetUInt32(bytes, "Value").Should().Be((uint)expected.Value); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsInt64Enum_ShouldSerializeValue()
        {
            var expected = new ClosedClass<Int64Enum>(Int64Enum.Max);   // Arrange
            var bytes = _serializer.Serialize(expected);                // Act
            GetInt64(bytes, "Value").Should().Be((long)expected.Value); // Assert
        }

        [Fact]
        public void Serialize_WhenValueIsUInt64Enum_ShouldSerializeValue()
        {
            var expected = new ClosedClass<UInt64Enum>(UInt64Enum.Max);   // Arrange
            var bytes = _serializer.Serialize(expected);                  // Act
            GetUInt64(bytes, "Value").Should().Be((ulong)expected.Value); // Assert
        }
        /*
        [Fact]
        public void Serialize_WhenValueIsNestedClass_ShouldSerializeValue()
        {
            var expected = new ClosedClass<ClosedClass<int>>(new ClosedClass<int>(int.MaxValue)); // Arrange
            var bytes = _serializer.Serialize(expected);                                          // Act
            GetInt32(bytes, "Value.Value").Should().Be(expected.Value.Value);                     // Assert
        }
        */
        [Fact]
        public void Deserialize_WhenNullableIsEnumAndMissing_ShouldSetValueToNull()
        {
            var bytes = Encoding.UTF8.GetBytes("{}");                            // Arrange
            var actual = _serializer.Deserialize<ClosedClass<Int8Enum?>>(bytes); // Act
            actual.Value.Should().BeNull();                                      // Assert
        }

        [Fact]
        public void Deserialize_WhenNullableIsEnumAndNull_ShouldSetValueToNull()
        {
            var bytes = Encoding.UTF8.GetBytes("{\"Value\": null}");             // Arrange
            var actual = _serializer.Deserialize<ClosedClass<Int8Enum?>>(bytes); // Act
            actual.Value.Should().BeNull();                                      // Assert
        }

        [Fact]
        public void Deserialize_WhenNullableIsEnumAndSet_ShouldSetValue()
        {
            var bytes = Encoding.UTF8.GetBytes("{\"Value\": 255}");              // Arrange
            var actual = _serializer.Deserialize<ClosedClass<Int8Enum?>>(bytes); // Act
            actual.Value.Should().Be(255);                                       // Assert
        }

        [Fact]
        public void Deserialize_WhenNullableIsMissing_ShouldSetValueToNull()
        {
            var bytes = Encoding.UTF8.GetBytes("{}");                       // Arrange
            var actual = _serializer.Deserialize<ClosedClass<int?>>(bytes); // Act
            actual.Value.Should().BeNull();                                 // Assert
        }

        [Fact]
        public void Deserialize_WhenNullableIsNull_ShouldSetValueToNull()
        {
            var bytes = Encoding.UTF8.GetBytes("{\"Value\": null}");        // Arrange
            var actual = _serializer.Deserialize<ClosedClass<int?>>(bytes); // Act
            actual.Value.Should().BeNull();                                 // Assert
        }

        [Fact]
        public void Deserialize_WhenNullableIsSet_ShouldSetValue()
        {
            var bytes = Encoding.UTF8.GetBytes("{\"Value\": 9}");           // Arrange
            var actual = _serializer.Deserialize<ClosedClass<int?>>(bytes); // Act
            actual.Value.Should().Be(9);                                    // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsBoolean_ShouldDeserializeValue()
        {
            var expected = new OpenClass<bool>(true); // Arrange
            var actual = Deserialize(expected);       // Act
            actual.Value.Should().Be(expected.Value); // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsByte_ShouldDeserializeValue()
        {
            var expected = new OpenClass<byte>(byte.MaxValue); // Arrange
            var actual = Deserialize(expected);                // Act
            actual.Value.Should().Be(expected.Value);          // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsSByte_ShouldDeserializeValue()
        {
            var expected = new OpenClass<sbyte>(sbyte.MaxValue); // Arrange
            var actual = Deserialize(expected);                  // Act
            actual.Value.Should().Be(expected.Value);            // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsInt16_ShouldDeserializeValue()
        {
            var expected = new OpenClass<short>(short.MaxValue); // Arrange
            var actual = Deserialize(expected);                  // Act
            actual.Value.Should().Be(expected.Value);            // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsUInt16_ShouldDeserializeValue()
        {
            var expected = new OpenClass<ushort>(ushort.MaxValue); // Arrange
            var actual = Deserialize(expected);                    // Act
            actual.Value.Should().Be(expected.Value);              // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsInt32_ShouldDeserializeValue()
        {
            var expected = new OpenClass<int>(int.MaxValue); // Arrange
            var actual = Deserialize(expected);              // Act
            actual.Value.Should().Be(expected.Value);        // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsUInt32_ShouldDeserializeValue()
        {
            var expected = new OpenClass<uint>(uint.MaxValue); // Arrange
            var actual = Deserialize(expected);                // Act
            actual.Value.Should().Be(expected.Value);          // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsInt64_ShouldDeserializeValue()
        {
            var expected = new OpenClass<long>(long.MaxValue); // Arrange
            var actual = Deserialize(expected);                // Act
            actual.Value.Should().Be(expected.Value);          // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsUInt64_ShouldDeserializeValue()
        {
            var expected = new OpenClass<ulong>(ulong.MaxValue); // Arrange
            var actual = Deserialize(expected);                  // Act
            actual.Value.Should().Be(expected.Value);            // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsFloat_ShouldDeserializeValue()
        {
            var expected = new OpenClass<float>(float.MaxValue); // Arrange
            var actual = Deserialize(expected);                  // Act
            actual.Value.Should().Be(expected.Value);            // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsDouble_ShouldDeserializeValue()
        {
            var expected = new OpenClass<double>(double.MaxValue); // Arrange
            var actual = Deserialize(expected);                    // Act
            actual.Value.Should().Be(expected.Value);              // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsDecimal_ShouldDeserializeValue()
        {
            var expected = new OpenClass<decimal>(decimal.MaxValue); // Arrange
            var actual = Deserialize(expected);                      // Act
            actual.Value.Should().Be(expected.Value);                // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsString_ShouldDeserializeValue()
        {
            var expected = new OpenClass<string>("test"); // Arrange
            var actual = Deserialize(expected);           // Act
            actual.Value.Should().Be(expected.Value);     // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsGuid_ShouldDeserializeValue()
        {
            var expected = new OpenClass<Guid>(Guid.NewGuid()); // Arrange
            var actual = Deserialize(expected);                 // Act
            actual.Value.Should().Be(expected.Value);           // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsDateTime_ShouldDeserializeValue()
        {
            var expected = new OpenClass<DateTime>(DateTime.MaxValue); // Arrange
            var actual = Deserialize(expected);                        // Act
            actual.Value.Should().Be(expected.Value);                  // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsDateTimeOffset_ShouldDeserializeValue()
        {
            var expected = new OpenClass<DateTimeOffset>(DateTimeOffset.MaxValue); // Arrange
            var actual = Deserialize(expected);                                    // Act
            actual.Value.Should().Be(expected.Value);                              // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsByteEnum_ShouldDeserializeValue()
        {
            var expected = new OpenClass<Int8Enum>(Int8Enum.Max); // Arrange
            var actual = Deserialize(expected);                   // Act
            actual.Value.Should().Be((byte)expected.Value);       // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsSByteEnum_ShouldDeserializeValue()
        {
            var expected = new OpenClass<UInt8Enum>(UInt8Enum.Max); // Arrange
            var actual = Deserialize(expected);                     // Act
            actual.Value.Should().Be((sbyte)expected.Value);        // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsInt16Enum_ShouldDeserializeValue()
        {
            var expected = new OpenClass<Int16Enum>(Int16Enum.Max); // Arrange
            var actual = Deserialize(expected);                     // Act
            actual.Value.Should().Be((short)expected.Value);         // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsUInt16Enum_ShouldDeserializeValue()
        {
            var expected = new OpenClass<UInt16Enum>(UInt16Enum.Max); // Arrange
            var actual = Deserialize(expected);                       // Act
            actual.Value.Should().Be((ushort)expected.Value);         // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsInt32Enum_ShouldDeserializeValue()
        {
            var expected = new OpenClass<Int32Enum>(Int32Enum.Max); // Arrange
            var actual = Deserialize(expected);                     // Act
            actual.Value.Should().Be((int)expected.Value);          // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsUInt32Enum_ShouldDeserializeValue()
        {
            var expected = new OpenClass<UInt32Enum>(UInt32Enum.Max); // Arrange
            var actual = Deserialize(expected);                       // Act
            actual.Value.Should().Be((uint)expected.Value);           // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsInt64Enum_ShouldDeserializeValue()
        {
            var expected = new OpenClass<Int64Enum>(Int64Enum.Max); // Arrange
            var actual = Deserialize(expected);                     // Act
            actual.Value.Should().Be((long)expected.Value);         // Assert
        }

        [Fact]
        public void Deserialize_WhenValueIsUInt64Enum_ShouldDeserializeValue()
        {
            var expected = new OpenClass<UInt64Enum>(UInt64Enum.Max); // Arrange
            var actual = Deserialize(expected);                       // Act
            actual.Value.Should().Be((ulong) expected.Value);         // Assert
        }

        [Fact]
        public void Deserialize_WhenParsingUnknownNull_ShouldIgnoreIt()
        {
            const int expected = int.MaxValue;                                                  // Arrange
            var actual = Deserialize<int>("{\"Unknown\": null, \"Value\": " + expected + " }"); // Act
            actual.Value.Should().Be(expected);                                                 // Assert
        }

        [Fact]
        public void Deserialize_WhenParsingUnknownBoolean_ShouldIgnoreIt()
        {
            const int expected = int.MaxValue;                                                  // Arrange
            var actual = Deserialize<int>("{\"Unknown\": true, \"Value\": " + expected + " }"); // Act
            actual.Value.Should().Be(expected);                                                 // Assert
        }

        [Fact]
        public void Deserialize_WhenParsingUnknownNumber_ShouldIgnoreIt()
        {
            const int expected = int.MaxValue;                                               // Arrange
            var actual = Deserialize<int>("{\"Unknown\": 9, \"Value\": " + expected + " }"); // Act
            actual.Value.Should().Be(expected);                                              // Assert
        }

        [Fact]
        public void Deserialize_WhenParsingUnknownString_ShouldIgnoreIt()
        {
            const int expected = int.MaxValue;                                                      // Arrange
            var actual = Deserialize<int>("{\"Unknown\": \"test\", \"Value\": " + expected + " }"); // Act
            actual.Value.Should().Be(expected);                                                     // Assert
        }

        [Fact]
        public void Deserialize_WhenParsingUnknownObject_ShouldIgnoreIt()
        {
            const int expected = int.MaxValue;                                                // Arrange
            var actual = Deserialize<int>("{\"Unknown\": {}, \"Value\": " + expected + " }"); // Act
            actual.Value.Should().Be(expected);                                               // Assert
        }

        [Fact]
        public void Deserialize_WhenParsingUnknownArray_ShouldIgnoreIt()
        {
            const int expected = int.MaxValue;                                                // Arrange
            var actual = Deserialize<int>("{\"Unknown\": [], \"Value\": " + expected + " }"); // Act
            actual.Value.Should().Be(expected);                                               // Assert
        }
    }
}
