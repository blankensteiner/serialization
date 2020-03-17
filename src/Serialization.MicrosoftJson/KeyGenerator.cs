namespace Serialization.MicrosoftJson
{
    using System;
    using System.Buffers;

    public sealed class KeyGenerator
    {
        public long Generate(byte[] bytes)
        {
            var key = new LongUnion
            {
                B0 = (byte)bytes.Length
            };

            for (var i = 0; i < bytes.Length; ++i)
            {
                switch (i)
                {
                    case 0:
                        key.B1 = bytes[i];
                        continue;
                    case 1:
                        key.B2 = bytes[i];
                        continue;
                    case 2:
                        key.B3 = bytes[i];
                        continue;
                    case 3:
                        key.B4 = bytes[i];
                        continue;
                    case 4:
                        key.B5 = bytes[i];
                        continue;
                    case 5:
                        key.B6 = bytes[i];
                        continue;
                    case 6:
                        key.B7 = bytes[i];
                        continue;
                }

                break;
            }

            return key.Long;
        }

        public long Generate(ReadOnlySpan<byte> span)
        {
            var key = new LongUnion
            {
                B0 = (byte)span.Length
            };

            for (var i = 0; i < span.Length; ++i)
            {
                switch (i)
                {
                    case 0:
                        key.B1 = span[i];
                        continue;
                    case 1:
                        key.B2 = span[i];
                        continue;
                    case 2:
                        key.B3 = span[i];
                        continue;
                    case 3:
                        key.B4 = span[i];
                        continue;
                    case 4:
                        key.B5 = span[i];
                        continue;
                    case 5:
                        key.B6 = span[i];
                        continue;
                    case 6:
                        key.B7 = span[i];
                        continue;
                }

                break;
            }

            return key.Long;
        }

        public long Generate(ReadOnlySequence<byte> sequence)
        {
            var key = new LongUnion
            {
                B0 = (byte)sequence.Length
            };

            var index = 0;

            foreach (var memory in sequence)
            {
                var span = memory.Span;
                for (var i = 0; i < span.Length; ++i, ++index)
                {
                    switch (index)
                    {
                        case 0:
                            key.B1 = span[i];
                            continue;
                        case 1:
                            key.B2 = span[i];
                            continue;
                        case 2:
                            key.B3 = span[i];
                            continue;
                        case 3:
                            key.B4 = span[i];
                            continue;
                        case 4:
                            key.B5 = span[i];
                            continue;
                        case 5:
                            key.B6 = span[i];
                            continue;
                        case 6:
                            key.B7 = span[i];
                            continue;
                    }

                    break;
                }

                if (index >= 6)
                    break;
            }

            return key.Long;
        }
    }
}
