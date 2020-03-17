namespace Serialization.MicrosoftJson.Abstractions
{
    public interface ISerializerBuilder
    {
        ITypeBuilder<TValue> Type<TValue>(int version = 0, bool fallback = true);
    }
}
