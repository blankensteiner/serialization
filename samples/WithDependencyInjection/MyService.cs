namespace WithDependencyInjection
{
    using Microsoft.Extensions.Hosting;
    using Serialization.Abstractions;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class MyService : BackgroundService
    {
        public MyService(ISerializer serializer, ISerializer<ClosedClass> serializerOfT)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes("{\"Boolean\": true}");
            var c1 = serializer.Deserialize<ClosedClass>(bytes);
            var c2 = serializerOfT.Deserialize(bytes);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
