namespace Serialization.MicrosoftJson.Abstractions
{
    public interface ISerializerBuilder
    {
        ITypeBuilder<TValue> AddType<TValue>(int version = 0, bool fallback = true);
    }
}
