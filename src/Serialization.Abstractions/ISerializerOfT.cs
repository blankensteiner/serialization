namespace Serialization.Abstractions
{
    using System;
    using System.Buffers;
    using System.IO;

    public interface ISerializer<TValue>
    {
        byte[] Serialize(TValue value);
        byte[] Serialize(TValue value, int version);
        void Serialize(Stream destination, TValue value);
        void Serialize(Stream destination, TValue value, int version);
        void Serialize(IBufferWriter<byte> destination, TValue value);
        void Serialize(IBufferWriter<byte> destination, TValue value, int version);
        TValue Deserialize(Stream data);
        TValue Deserialize(Stream data, int version);
        TValue Deserialize(byte[] data);
        TValue Deserialize(byte[] data, int version);
        TValue Deserialize(ReadOnlySpan<byte> data);
        TValue Deserialize(ReadOnlySpan<byte> data, int version);
        TValue Deserialize(ReadOnlyMemory<byte> data);
        TValue Deserialize(ReadOnlyMemory<byte> data, int version);
        TValue Deserialize(ReadOnlySequence<byte> data);
        TValue Deserialize(ReadOnlySequence<byte> data, int version);
    }
}
