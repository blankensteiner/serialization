namespace Serialization.MicrosoftJson.DependencyInjection
{
    using Microsoft.Extensions.DependencyInjection;
    using Serialization.Abstractions;
    using Serialization.MicrosoftJson.Abstractions;
    using System;
    using System.Linq;
    using System.Text.Json;

    public static class ServiceCollectionExtensions
    {
        public static void AddSerialization(this IServiceCollection services, Action<JsonSerializerOptions, ISerializerBuilder> builder)
        {
            var jsonSerializerOptions = new JsonSerializerOptions();
            var serializerBuilder = new SerializerBuilder(jsonSerializerOptions);

            builder(jsonSerializerOptions, serializerBuilder);

            var serializer = serializerBuilder.Build();
            services.AddSingleton<ISerializer>(serializer);

            foreach (var serializerOfT in serializer.GetAllSerializers())
            {
                services.AddSingleton(serializerOfT.GetType().GetInterfaces().First(), serializerOfT);
            }
        }
    }
}
