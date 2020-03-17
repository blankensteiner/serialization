namespace Serialization.ProtobufNet
{
    using Serialization.Abstractions;
    using System;
    using System.Buffers;
    using System.IO;

    public sealed class Serializer : ISerializer
    {
        public Serializer()
        {
            var someType = typeof(string);
            ProtoBuf.Meta.RuntimeTypeModel.Default.Add(someType, false).Add(1, "_createdOn").UseConstructor = false;
        }

        public TValue Deserialize<TValue>(Stream data)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize<TValue>(Stream data, int version)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize<TValue>(byte[] data)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize<TValue>(byte[] data, int version)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize<TValue>(ReadOnlySpan<byte> data)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize<TValue>(ReadOnlySpan<byte> data, int version)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize<TValue>(ReadOnlyMemory<byte> data)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize<TValue>(ReadOnlyMemory<byte> data, int version)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize<TValue>(ReadOnlySequence<byte> data)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize<TValue>(ReadOnlySequence<byte> data, int version)
        {
            throw new NotImplementedException();
        }

        public ISerializer<TValue> GetSerializerFor<TValue>()
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize<TValue>(TValue value)
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize<TValue>(TValue value, int version)
        {
            throw new NotImplementedException();
        }

        public void Serialize<TValue>(Stream destination, TValue value)
        {
            throw new NotImplementedException();
        }

        public void Serialize<TValue>(Stream destination, TValue value, int version)
        {
            throw new NotImplementedException();
        }

        public void Serialize<TValue>(IBufferWriter<byte> destination, TValue value)
        {
            throw new NotImplementedException();
        }

        public void Serialize<TValue>(IBufferWriter<byte> destination, TValue value, int version)
        {
            throw new NotImplementedException();
        }
    }
}
