# Serialization

---
> NOTICE: This project is alpha and therefore there are currently no NuGet packages.
---

Deserializing and serializing DTOs is something every serialization framework supports, but for the DDD practitioners, there's a different need when persisting aggregates.
This project will let you serialize and deserialize private and readonly fields without calling a constructor.

## Philosophy

Freedom, abstraction and version awareness are the three cornerstones of this project.

### Freedom

We want the freedom to model our aggregates as we see fit and not having to pull in weavers or NuGet packages into our domain project. Simply put, we want:

* No forced attributes
* No forced inheritance
* No forced constructors
* No forced conventions

In short, we just don't want any constraints.

### Serialization abstractions

Microsoft has done a great job in creating interfaces for [logging](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Abstractions/) and [dependency injection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions/) and soon we will also have vendor-agnostic interfaces for metrics and tracing thanks to [OpenTelemetry](https://opentelemetry.io/).
We feel that a similar standard is needed for serialization and therefore we have created Serialization.Abstractions, defining two interfaces. ISerializer for (de)serializing all types and ISerializer\<T\> for (de)serializing a specific type. There is a small performance improvement when using ISerializer\<T\>.

### Version awareness

As more and more are moving to document-style persistence, doing canary/rolling deployments and having larger and larger databases the need for versioning your aggregates increase and so does your ability to handle multiple versions. This is a feature that's a good fit for a serialization framework but it's sadly missing in most other frameworks.

We would like to make it easier to

* Load an older version and deserialize it into the current version.
* Load a newer version and merge the changes back into the data, to prevent deleting new and unknown fields.

## What encodings are supported?

We have started with JSON (Serialization.MicrosoftJson) building upon [Utf8JsonReader](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.utf8jsonreader?view=netcore-3.1) and [Utf8JsonWriter](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.utf8jsonwriter?view=netcore-3.1) from [System.Text.Json](https://www.nuget.org/packages/System.Text.Json/).
At some point we will support [protobuf](https://developers.google.com/protocol-buffers) (using [protobuf-net](https://github.com/protobuf-net/protobuf-net)) and [MsgPack](https://msgpack.org/) (using [MessagePack](https://github.com/neuecc/MessagePack-CSharp)).

## Getting Started

As mentioned there are currently no NuGet packages, but let's have a look at how to get an ISerializer or ISeralizer\<T\>.

### With manual creation

Let start with building an ISerializer.

```csharp
var builder = new SerializerBuilder();
builder.Type<ClosedClass>();
var serializer = builder.Build();
```

If you then want a type-specific serializer (ISerializer\<T\>) all you have to do is.

```csharp
var serializerOfT = serializer.GetSerializerFor<ClosedClass>();
```

### With dependency injection

In the Serialization.MicrosoftJson.DependencyInjection namespace there is an IServiceCollection extension for setting up serialization.

```csharp
await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSerialization((options, builder) =>
        {
            builder.Type<ClosedClass>();
        });
        services.AddHostedService<MyService>();
    })
    .Build()
    .RunAsync();
```

All there is left to do now is to either add ISerializer or ISerializer\<T\> to your services.

```csharp
public sealed class MyService : BackgroundService
{
    public MyService(ISerializer serializer, ISerializer<ClosedClass> serializerOfT) { }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) 
        => throw new NotImplementedException();
}
```

## Supported types

We are just getting started but currently, we have support for:

- [X] Boolean
- [X] Signed and unsigned byte
- [X] Signed and unsigned short
- [X] Signed and unsigned integer
- [X] Signed and unsigned long
- [X] Float
- [X] Double
- [X] Decimal
- [X] String
- [X] Guid
- [X] DateTime
- [X] DateTimeOffset
- [X] Nullable<>
- [X] Enum

## Roadmap

Next we will look into supporting:

- [ ] Char
- [ ] TimeSpan
- [ ] Uri
- [ ] Custom types
- [ ] Array[] and collections
- [ ] Dictionary
- [ ] Version?
- [ ] Complex?
- [ ] BigInteger?
- [ ] BitArray?

## Benchmarks

Looking at performance is always exciting so naturally, we created benchmarks to measure serialization and deserialization.
Microsoft's [JsonSerializer](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializer?view=netcore-3.1) is using a DTO called OpenClass (consisting only of public properties with public getters and setters) and we use a ClosedClass (consisting only of private and readonly fields).
As you can see below the performance and GC load is the same.

### Serializing

```csharp
[Benchmark] public byte[] System_Text_Json_JsonSerializer()
    => System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(_openClass);

[Benchmark] public byte[] Serialization_MicrosoftJson_Serializer()
    => _serializer.Serialize(_closedClass);

[Benchmark] public byte[] Serialization_MicrosoftJson_Serializer_Of_T()
    => _serializerOfT.Serialize(_closedClass);
```

``` ini
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18363
Intel Core i5-7500 CPU 3.40GHz (Kaby Lake), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.1.200
  [Host]     : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT
  DefaultJob : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT
```

|                                      Method |     Mean |     Error |    StdDev | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------------------------------------- |---------:|----------:|----------:|-----:|-------:|------:|------:|----------:|
| Serialization_MicrosoftJson_Serializer_Of_T | 1.611 us | 0.0072 us | 0.0063 us |    1 | 0.1469 |     - |     - |     464 B |
|             System_Text_Json_JsonSerializer | 1.659 us | 0.0192 us | 0.0170 us |    2 | 0.1469 |     - |     - |     464 B |
|      Serialization_MicrosoftJson_Serializer | 1.738 us | 0.0068 us | 0.0057 us |    3 | 0.1450 |     - |     - |     464 B |

Always nice to be #1, but honestly even the performance difference between #1 and #3 practically doesn't matter.

### Deserializing

```csharp
[Benchmark] public OpenClass System_Text_Json_JsonSerializer()
    => System.Text.Json.JsonSerializer.Deserialize<OpenClass>(_bytes);

[Benchmark] public ClosedClass Serialization_MicrosoftJson_Serializer()
    => _serializer.Deserialize<ClosedClass>(_bytes);

[Benchmark] public ClosedClass Serialization_MicrosoftJson_SerializerOfT()
    => _serializerOfT.Deserialize(_bytes);
```

``` ini
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18363
Intel Core i5-7500 CPU 3.40GHz (Kaby Lake), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.1.200
  [Host]     : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT
  DefaultJob : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT
```

|                                    Method |     Mean |     Error |    StdDev | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------------------------ |---------:|----------:|----------:|-----:|-------:|------:|------:|----------:|
| Serialization_MicrosoftJson_SerializerOfT | 2.556 us | 0.0135 us | 0.0126 us |    1 | 0.0496 |     - |     - |     160 B |
|           System_Text_Json_JsonSerializer | 2.587 us | 0.0091 us | 0.0085 us |    1 | 0.0496 |     - |     - |     160 B |
|    Serialization_MicrosoftJson_Serializer | 2.637 us | 0.0123 us | 0.0115 us |    2 | 0.0496 |     - |     - |     160 B |

Notice how [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) rank both Serialization_MicrosoftJson_SerializerOfT and System_Text_Json_JsonSerializer as #1, because the performance difference is insignificant.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/blankensteiner/serialization/tags).

## Authors

* **Daniel Blankensteiner** - *Initial work*

See also the list of [contributors](https://github.com/blankensteiner/serialization/contributors) who participated in this project.

## License

This project is licensed under the MIT License (MIT) - see the [LICENSE](LICENSE) file for details.
