namespace Serialization.MicrosoftJson
{
    using Serialization.Abstractions;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.IO;

    public sealed class Serializer : ISerializer
    {
        private readonly Dictionary<Type, object> _serializers;

        public Serializer(Dictionary<Type, object> serializers) => _serializers = serializers;

        public TValue Deserialize<TValue>(Stream data) => GetSerializerFor<TValue>().Deserialize(data);
        public TValue Deserialize<TValue>(Stream data, int version) => GetSerializerFor<TValue>().Deserialize(data, version);
        public TValue Deserialize<TValue>(byte[] data) => GetSerializerFor<TValue>().Deserialize(data);
        public TValue Deserialize<TValue>(byte[] data, int version) => GetSerializerFor<TValue>().Deserialize(data, version);
        public TValue Deserialize<TValue>(ReadOnlySpan<byte> data) => GetSerializerFor<TValue>().Deserialize(data);
        public TValue Deserialize<TValue>(ReadOnlySpan<byte> data, int version) => GetSerializerFor<TValue>().Deserialize(data, version);
        public TValue Deserialize<TValue>(ReadOnlyMemory<byte> data) => GetSerializerFor<TValue>().Deserialize(data);
        public TValue Deserialize<TValue>(ReadOnlyMemory<byte> data, int version) => GetSerializerFor<TValue>().Deserialize(data, version);
        public TValue Deserialize<TValue>(ReadOnlySequence<byte> data) => GetSerializerFor<TValue>().Deserialize(data);
        public TValue Deserialize<TValue>(ReadOnlySequence<byte> data, int version) => GetSerializerFor<TValue>().Deserialize(data, version);

        public byte[] Serialize<TValue>(TValue value) => GetSerializerFor<TValue>().Serialize(value);
        public byte[] Serialize<TValue>(TValue value, int version) => GetSerializerFor<TValue>().Serialize(value, version);
        public void Serialize<TValue>(Stream destination, TValue value) => GetSerializerFor<TValue>().Serialize(destination, value);
        public void Serialize<TValue>(Stream destination, TValue value, int version) => GetSerializerFor<TValue>().Serialize(destination, value, version);
        public void Serialize<TValue>(IBufferWriter<byte> destination, TValue value) => GetSerializerFor<TValue>().Serialize(destination, value);
        public void Serialize<TValue>(IBufferWriter<byte> destination, TValue value, int version) => GetSerializerFor<TValue>().Serialize(destination, value, version);

        public ISerializer<TValue> GetSerializerFor<TValue>()
        {
            var serializer = _serializers[typeof(TValue)];

            if (serializer is null)
                throw new Exception($"No serializer was found for the type '{typeof(TValue)}'");

            return (ISerializer<TValue>) serializer;
        }

        public IEnumerable<object> GetAllSerializers() => _serializers.Values;
    }
}
