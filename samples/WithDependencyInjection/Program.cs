namespace WithDependencyInjection
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serialization.MicrosoftJson.DependencyInjection;
    using System.Threading.Tasks;

    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSerialization((options, builder) =>
                    {
                        builder.AddType<ClosedClass>();
                    });

                    services.AddHostedService<MyService>();
                })
                .Build()
                .RunAsync();
        }
    }
}
