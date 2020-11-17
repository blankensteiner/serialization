namespace Serialization.MicrosoftJson
{
    using Serialization.Abstractions;
    using Serialization.MicrosoftJson.Abstractions;
    using Serialization.MicrosoftJson.Extensions;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.Serialization;
    using System.Text.Json;

    public sealed class TypeSerializer<TValue> : ISerializer<TValue>, ITypeSerializer<TValue>
    {
        private readonly Type _type;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly JsonWriterOptions _writerOptions;
        private readonly JsonReaderOptions _readerOptions;
        private readonly ByteArrayKeyDictionary<ReaderToFieldSetter> _setters;
        private readonly Serializer _serializer;

        private delegate void ReaderToFieldSetter(ref Utf8JsonReader reader, TValue value);
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

            _setters = new ByteArrayKeyDictionary<ReaderToFieldSetter>();
            var fieldMap = new Dictionary<string, FieldInfo>();

            foreach (var mapping in configuration.Mappings)
            {
                if (mapping.PropertyName is null)
                {
                    var name = mapping.Field.Name;
                    var startIndex = name[0] == '_' ? 1 : 0;
                    name = char.ToUpper(name[startIndex]) + name.Substring(++startIndex); // Default to pascal casing

                    if (serializerOptions.PropertyNamingPolicy is not null)
                        name = serializerOptions.PropertyNamingPolicy.ConvertName(name);

                    mapping.PropertyName = name;
                }

                var propertyNameBytes = System.Text.Encoding.UTF8.GetBytes(mapping.PropertyName);
                var fieldType = mapping.Field.GetUnderlyingType();
                _setters.Add(propertyNameBytes, CreateReaderToFieldSetter(getterMethods[fieldType], mapping.Field));
                fieldMap.Add(mapping.PropertyName, mapping.Field);
            }

            _serializer = CreateSerializer(fieldMap, writerMethods);
        }

        private static ReaderToFieldSetter CreateReaderToFieldSetter(MethodInfo methodInfo, FieldInfo fieldInfo)
        {
            var valueType = typeof(TValue);
            var parameterTypes = new Type[] { typeof(Utf8JsonReader).MakeByRefType(), valueType };
            var dm = new DynamicMethod(valueType.Name + "Set" + fieldInfo.Name, typeof(void), parameterTypes);
            var il = dm.GetILGenerator();

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, methodInfo);

            var underlyingType = Nullable.GetUnderlyingType(fieldInfo.FieldType);
            if (underlyingType is not null)
            {
                var nullableType = typeof(Nullable<>).MakeGenericType(underlyingType);
                il.Emit(OpCodes.Newobj, nullableType.GetRequiredConstructor(new[] { underlyingType }));
            }

            il.Emit(OpCodes.Stfld, fieldInfo);
            il.Emit(OpCodes.Ret);

            return (ReaderToFieldSetter) dm.CreateDelegate(typeof(ReaderToFieldSetter));
        }

        private static Serializer CreateSerializer(Dictionary<string, FieldInfo> fieldMap, Dictionary<Type, MethodInfo> writerMethods)
        {
            var writerType = typeof(Utf8JsonWriter);
            var valueType = typeof(TValue);

            var parameterTypes = new[] { writerType, valueType };
            var dm = new DynamicMethod(valueType.Name + "Serializer", typeof(void), parameterTypes, valueType, true);
            var il = dm.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);

            il.Emit(OpCodes.Callvirt, writerType.GetRequiredMethod("WriteStartObject"));

            foreach (var map in fieldMap)
            {
                var underlyingType = Nullable.GetUnderlyingType(map.Value.FieldType);
                if (underlyingType is not null)
                {
                    var skipIfNullableIsNull = il.DefineLabel();
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldflda, map.Value);
                    var nullableType = typeof(Nullable<>).MakeGenericType(underlyingType);
                    var hasValueProperty = nullableType.GetRequiredProperty("HasValue");
                    il.Emit(OpCodes.Call, hasValueProperty.GetRequiredGetMethod());
                    il.Emit(OpCodes.Brfalse, skipIfNullableIsNull);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldstr, map.Key);
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldflda, map.Value);
                    var valueProperty = nullableType.GetRequiredProperty("Value");
                    il.Emit(OpCodes.Call, valueProperty.GetRequiredGetMethod());
                    il.Emit(OpCodes.Callvirt, writerMethods[map.Value.GetUnderlyingType()]);
                    il.MarkLabel(skipIfNullableIsNull);
                }
                else
                {
                    var skipIfClassAndNull = il.DefineLabel();
                    var isClass = map.Value.FieldType.IsClass;

                    if (isClass)
                    {
                        il.Emit(OpCodes.Ldarg_1);
                        il.Emit(OpCodes.Ldfld, map.Value);
                        il.Emit(OpCodes.Brfalse_S, skipIfClassAndNull);
                    }

                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldstr, map.Key);
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldfld, map.Value);
                    il.Emit(OpCodes.Callvirt, writerMethods[map.Value.GetUnderlyingType()]);

                    if (isClass)
                        il.MarkLabel(skipIfClassAndNull);
                }
            }

            il.Emit(OpCodes.Ldarg_0);

            il.Emit(OpCodes.Callvirt, writerType.GetRequiredMethod("WriteEndObject"));
            il.Emit(OpCodes.Ret);
            return (Serializer) dm.CreateDelegate(typeof(Serializer));
        }

        public TValue Deserialize(ref Utf8JsonReader reader)
        {
            var value = (TValue) FormatterServices.GetUninitializedObject(_type);

            while (reader.Read())
            {
                if (reader.TokenType != JsonTokenType.PropertyName)
                    continue;

                var setter = reader.HasValueSequence ? _setters.GetValue(reader.ValueSequence) : _setters.GetValue(reader.ValueSpan);
                if (setter is null)
                {
                    reader.Skip();
                    continue;
                }

                reader.Read();

                if (reader.TokenType == JsonTokenType.Null)
                    continue;

                setter(ref reader, value);
            }

            return value;
        }

        public void Serialize(Utf8JsonWriter writer, TValue value) => _serializer(writer, value);

        public TValue Deserialize(byte[] data)
        {
            var reader = new Utf8JsonReader(data, _readerOptions);
            return Deserialize(ref reader);
        }

        public byte[] Serialize(TValue value)
        {
            using var bufferWriter = new PooledByteBufferWriter(_serializerOptions.DefaultBufferSize);
            using (var writer = new Utf8JsonWriter(bufferWriter))
            {
                Serialize(writer, value);
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
