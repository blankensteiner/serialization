namespace Serialization.MicrosoftJson
{
    using Serialization.MicrosoftJson.Extensions;
    using System;
    using System.Buffers;
    using System.Collections.Generic;

    public sealed class ByteArrayKeyDictionary<T> where T : class
    {
        private readonly Dictionary<long, List<KeyValue>> _keyValues;
        private readonly KeyGenerator _keyGenerator;

        public ByteArrayKeyDictionary()
        {
            _keyValues = new Dictionary<long, List<KeyValue>>();
            _keyGenerator = new KeyGenerator();
        }

        public void Add(byte[] key, T value)
        {
            var generatedKey = _keyGenerator.Generate(key);

            var keyValue = new KeyValue(key, value);

            if (_keyValues.ContainsKey(generatedKey))
                _keyValues[generatedKey].Add(keyValue);
            else
                _keyValues[generatedKey] = new List<KeyValue> { keyValue };
        }

        public T? GetValue(ReadOnlySpan<byte> key)
        {
            var generatedKey = _keyGenerator.Generate(key);

            if (_keyValues.TryGetValue(generatedKey, out var keyValues))
            {
                for (var i = 0; i < keyValues.Count; ++i)
                {
                    var keyValue = keyValues[i];
                    if (key.SequenceEqual(keyValue.Key))
                    {
                        return keyValue.Value;
                    }
                }
            }

            return null;
        }

        public T? GetValue(ReadOnlySequence<byte> key)
        {
            var generatedKey = _keyGenerator.Generate(key);

            if (_keyValues.TryGetValue(generatedKey, out var keyValues))
            {
                for (var i = 0; i < keyValues.Count; ++i)
                {
                    var keyValue = keyValues[i];
                    if (key.SequenceEqual(keyValue.Key))
                    {
                        return keyValue.Value;
                    }
                }
            }

            return null;
        }

        private sealed class KeyValue
        {
            public KeyValue(byte[] key, T value)
            {
                Key = key;
                Value = value;
            }

            public readonly byte[] Key;
            public readonly T Value;
        }
    }
}
