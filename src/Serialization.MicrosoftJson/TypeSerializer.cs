namespace Serialization.MicrosoftJson
{
    using Serialization.Abstractions;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.Serialization;
    using System.Text.Json;

    public sealed class TypeSerializer<TValue> : ISerializer<TValue>
    {
        private readonly Type _type;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly JsonWriterOptions _writerOptions;
        private readonly JsonReaderOptions _readerOptions;
        private readonly ByteArrayKeyDictionary<Setter> _dataKeyMap;
        private readonly Serializer _serializer;

        private delegate void Setter(ref Utf8JsonReader reader, TValue value);
        private delegate void Serializer(Utf8JsonWriter writer, TValue value);

        public TypeSerializer(
            TypeConfiguration configuration,
            JsonSerializerOptions serializerOptions,
            JsonWriterOptions writerOptions,
            JsonReaderOptions readerOptions,
            Dictionary<Type, MethodInfo> getterMethods,
            Dictionary<Type, MethodInfo> writerMethods)
        {
            _type = typeof(TValue);
            _serializerOptions = serializerOptions;
            _writerOptions = writerOptions;
            _readerOptions = readerOptions;

            _dataKeyMap = new ByteArrayKeyDictionary<Setter>();
            var fieldMap = new Dictionary<string, FieldInfo>();

            foreach (var mapping in configuration.Mappings)
            {
                if (mapping.PropertyName is null)
                {
                    var name = mapping.Field.Name;
                    var startIndex = name[0] == '_' ? 1 : 0;
                    name = char.ToUpper(name[startIndex]) + name.Substring(++startIndex); // Default to pascal casing

                    if (serializerOptions.PropertyNamingPolicy != null)
                        name = serializerOptions.PropertyNamingPolicy.ConvertName(name);

                    mapping.PropertyName = name;
                }

                var propertyNameBytes = System.Text.Encoding.UTF8.GetBytes(mapping.PropertyName);
                _dataKeyMap.Add(propertyNameBytes, CreateSetter(getterMethods[mapping.Field.FieldType], mapping.Field));
                fieldMap.Add(mapping.PropertyName, mapping.Field);
            }

            _serializer = CreateSerializer(fieldMap, writerMethods);
        }

        private static Setter CreateSetter(MethodInfo methodInfo, FieldInfo fieldInfo)
        {
            var dm = new DynamicMethod("Set" + fieldInfo.Name, typeof(void), new Type[] { typeof(Utf8JsonReader).MakeByRefType(), typeof(TValue) });
            var il = dm.GetILGenerator();
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, methodInfo);
            il.Emit(OpCodes.Stfld, fieldInfo);
            il.Emit(OpCodes.Ret);
            return (Setter)dm.CreateDelegate(typeof(Setter));
        }

        private static Serializer CreateSerializer(Dictionary<string, FieldInfo> fieldMap, Dictionary<Type, MethodInfo> writerMethods)
        {
            var parameters = new[] { typeof(Utf8JsonWriter), typeof(TValue) };
            var getter = new DynamicMethod("Getter", typeof(void), parameters, typeof(TValue), true);
            var il = getter.GetILGenerator();

            foreach (var map in fieldMap)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldstr, map.Key);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldfld, map.Value);
                il.Emit(OpCodes.Callvirt, writerMethods[map.Value.FieldType]);
            }

            il.Emit(OpCodes.Ret);
            return (Serializer)getter.CreateDelegate(typeof(Serializer));
        }

        public TValue Deserialize(byte[] data)
        {
            var value = (TValue)FormatterServices.GetUninitializedObject(_type);

            var reader = new Utf8JsonReader(data, _readerOptions);

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        Setter setter;
                        bool found = false;

                        if (reader.HasValueSequence)
                            found = _dataKeyMap.TryGetValue(reader.ValueSequence, out setter);
                        else
                            found = _dataKeyMap.TryGetValue(reader.ValueSpan, out setter);

                        if (found)
                        {
                            reader.Read();
                            if (reader.TokenType == JsonTokenType.Null && _serializerOptions.IgnoreNullValues)
                                continue;
                            setter(ref reader, value);
                        }
                        break;
                }
            }

            return value;
        }

        public byte[] Serialize(TValue value)
        {
            using var bufferWriter = new PooledByteBufferWriter(_serializerOptions.DefaultBufferSize);
            using (var writer = new Utf8JsonWriter(bufferWriter))
            {
                writer.WriteStartObject();
                _serializer(writer, value);
                writer.WriteEndObject();
            }
            return bufferWriter.WrittenMemory.ToArray();
        }

        public byte[] Serialize(TValue value, int version)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Stream destination, TValue value)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Stream destination, TValue value, int version)
        {
            throw new NotImplementedException();
        }

        public void Serialize(IBufferWriter<byte> destination, TValue value)
        {
            throw new NotImplementedException();
        }

        public void Serialize(IBufferWriter<byte> destination, TValue value, int version)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize(Stream data)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize(Stream data, int version)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize(byte[] data, int version)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize(ReadOnlySpan<byte> data)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize(ReadOnlySpan<byte> data, int version)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize(ReadOnlyMemory<byte> data)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize(ReadOnlyMemory<byte> data, int version)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize(ReadOnlySequence<byte> data)
        {
            throw new NotImplementedException();
        }

        public TValue Deserialize(ReadOnlySequence<byte> data, int version)
        {
            throw new NotImplementedException();
        }
    }
}
