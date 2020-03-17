namespace Serialization.MicrosoftJson.Abstractions
{
    public interface ITypeBuilder<TValue>
    {
        ITypeBuilder<TValue> Map(string fieldName, string propertyName);
        ITypeBuilder<TValue> Ignore(string fieldName);
    }
}
