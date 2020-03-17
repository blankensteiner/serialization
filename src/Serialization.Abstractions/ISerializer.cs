namespace Serialization.Abstractions
{
    using System;
    using System.Buffers;
    using System.IO;

    public interface ISerializer
    {
        byte[] Serialize<TValue>(TValue value);
        byte[] Serialize<TValue>(TValue value, int version);
        void Serialize<TValue>(Stream destination, TValue value);
        void Serialize<TValue>(Stream destination, TValue value, int version);
        void Serialize<TValue>(IBufferWriter<byte> destination, TValue value);
        void Serialize<TValue>(IBufferWriter<byte> destination, TValue value, int version);
        TValue Deserialize<TValue>(Stream data);
        TValue Deserialize<TValue>(Stream data, int version);
        TValue Deserialize<TValue>(byte[] data);
        TValue Deserialize<TValue>(byte[] data, int version);
        TValue Deserialize<TValue>(ReadOnlySpan<byte> data);
        TValue Deserialize<TValue>(ReadOnlySpan<byte> data, int version);
        TValue Deserialize<TValue>(ReadOnlyMemory<byte> data);
        TValue Deserialize<TValue>(ReadOnlyMemory<byte> data, int version);
        TValue Deserialize<TValue>(ReadOnlySequence<byte> data);
        TValue Deserialize<TValue>(ReadOnlySequence<byte> data, int version);

        ISerializer<TValue> GetSerializerFor<TValue>();
    }
}
