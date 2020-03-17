namespace Serialization.MicrosoftJson.Extensions
{
    using System.Buffers;

    public static class ReadOnlySequenceExtensions
    {
        public static bool SequenceEqual(this ReadOnlySequence<byte> sequence, byte[] bytes) // This assumes same length
        {
            var index = 0;

            foreach (var memory in sequence)
            {
                var span = memory.Span;

                for (var i = 0; i < span.Length; ++i, ++index)
                {
                    if (span[i] != bytes[index])
                        return false;
                }
            }

            return true;
        }
    }
}
