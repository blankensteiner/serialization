namespace Serialization.MicrosoftJson.Abstractions
{
    using System.Text.Json;

    internal interface ITypeSerializer<TValue>
    {
        TValue Deserialize(ref Utf8JsonReader reader);
        void Serialize(Utf8JsonWriter writer, TValue value);
    }
}
