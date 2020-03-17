namespace Serialization.MicrosoftJson.Benchmarks
{
    using BenchmarkDotNet.Running;

    class Program
    {
        static void Main(string[] args)
        {
            _ = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
