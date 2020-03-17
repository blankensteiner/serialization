namespace Serialization.MicrosoftJson
{
    using Serialization.MicrosoftJson.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text.Json;

    public sealed class SerializerBuilder : ISerializerBuilder
    {
        private readonly JsonSerializerOptions _options;
        private readonly List<TypeConfiguration> _typeConfigurations;

        public SerializerBuilder() : this(new JsonSerializerOptions()) { }

        public SerializerBuilder(JsonSerializerOptions options)
        {
            _options = options;
            _typeConfigurations = new List<TypeConfiguration>();
        }

        public ITypeBuilder<TValue> Type<TValue>(int version = 0, bool fallback = true)
        {
            var typeConfiguration = new TypeConfiguration(typeof(TValue), version, fallback);
            _typeConfigurations.Add(typeConfiguration);
            return new TypeBuilder<TValue>(typeConfiguration);
        }

        public Serializer Build()
        {
            var writerOptions = new JsonWriterOptions
            {
                Encoder = _options.Encoder,
                Indented = _options.WriteIndented
            };

            var readerOptions = new JsonReaderOptions
            {
                AllowTrailingCommas = _options.AllowTrailingCommas,
                CommentHandling = _options.ReadCommentHandling,
                MaxDepth = _options.MaxDepth
            };

            var serializers = new Dictionary<Type, object>();
            var getterMethods = LoadGetterMethods();
            var writerMethods = LoadWriterMethods();

            foreach (var tc in _typeConfigurations)
            {
                var typeSerializer = typeof(TypeSerializer<>).MakeGenericType(tc.Type);
                var instance = Activator.CreateInstance(typeSerializer, tc, _options, writerOptions, readerOptions, getterMethods, writerMethods);
                if (instance is null)
                    throw new Exception("Activator.CreateInstance returned null when trying to create: " + tc.Type.FullName);

                serializers[tc.Type] = instance;
            }

            return new Serializer(serializers);
        }

        private static Dictionary<Type, MethodInfo> LoadGetterMethods()
        {
            var getterMethods = new Dictionary<Type, MethodInfo>();

            void AddGetterMethod(Type type, string methodName)
            {
                var method = typeof(Utf8JsonReader).GetMethod(methodName);
                if (method is null)
                    throw new MissingMethodException(nameof(Utf8JsonReader), methodName);

                getterMethods.Add(type, method);
            }

            AddGetterMethod(typeof(bool), "GetBoolean");

            //AddGetterMethod(typeof(char), "GetChar");
            AddGetterMethod(typeof(string), "GetString");
            //AddGetterMethod(typeof(Uri), "GetUri");
            AddGetterMethod(typeof(Guid), "GetGuid");
            //AddGetterMethod(typeof(TimeSpan), "GetTimeSpan");
            AddGetterMethod(typeof(DateTime), "GetDateTime");
            AddGetterMethod(typeof(DateTimeOffset), "GetDateTimeOffset");

            AddGetterMethod(typeof(byte), "GetByte");
            AddGetterMethod(typeof(sbyte), "GetSByte");
            AddGetterMethod(typeof(short), "GetInt16");
            AddGetterMethod(typeof(ushort), "GetUInt16");
            AddGetterMethod(typeof(int), "GetInt32");
            AddGetterMethod(typeof(uint), "GetUInt32");
            AddGetterMethod(typeof(long), "GetInt64");
            AddGetterMethod(typeof(ulong), "GetUInt64");
            AddGetterMethod(typeof(float), "GetSingle");
            AddGetterMethod(typeof(double), "GetDouble");
            AddGetterMethod(typeof(decimal), "GetDecimal");
            
            return getterMethods;
        }

        private static Dictionary<Type, MethodInfo> LoadWriterMethods()
        {
            var writerMehods = new Dictionary<Type, MethodInfo>();

            void AddWriterMethod(string methodName, Type type)
            {
                var lookup = type;
                if (type == typeof(byte) || type == typeof(sbyte) || type == typeof(short) || type == typeof(ushort))
                    lookup = typeof(int);

                var method = typeof(Utf8JsonWriter).GetMethod(methodName, new Type[] { typeof(string), lookup });
                if (method is null)
                    throw new MissingMethodException(nameof(Utf8JsonWriter), methodName);

                writerMehods.Add(type, method);
            }

            AddWriterMethod("WriteBoolean", typeof(bool));

            //AddWriterMethod("WriteString", typeof(char));
            AddWriterMethod("WriteString", typeof(string));
            //AddWriterMethod("WriteString", typeof(Uri));
            AddWriterMethod("WriteString", typeof(Guid));
            //AddWriterMethod("WriteString", typeof(TimeSpan));
            AddWriterMethod("WriteString", typeof(DateTime));
            AddWriterMethod("WriteString", typeof(DateTimeOffset));

            AddWriterMethod("WriteNumber", typeof(byte));
            AddWriterMethod("WriteNumber", typeof(sbyte));
            AddWriterMethod("WriteNumber", typeof(short));
            AddWriterMethod("WriteNumber", typeof(ushort));
            AddWriterMethod("WriteNumber", typeof(int));
            AddWriterMethod("WriteNumber", typeof(uint));
            AddWriterMethod("WriteNumber", typeof(long));
            AddWriterMethod("WriteNumber", typeof(ulong));
            AddWriterMethod("WriteNumber", typeof(float));
            AddWriterMethod("WriteNumber", typeof(double));
            AddWriterMethod("WriteNumber", typeof(decimal));

            return writerMehods;
        }
    }
}
