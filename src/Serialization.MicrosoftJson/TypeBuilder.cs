namespace Serialization.MicrosoftJson
{
    using Serialization.MicrosoftJson.Abstractions;

    public sealed class TypeBuilder<TValue> : ITypeBuilder<TValue>
    {
        private readonly TypeConfiguration _typeConfiguration;

        public TypeBuilder(TypeConfiguration typeConfiguration)
        {
            _typeConfiguration = typeConfiguration;
        }

        public ITypeBuilder<TValue> Ignore(string fieldName)
        {
            _typeConfiguration.Remove(fieldName);
            return this;
        }

        public ITypeBuilder<TValue> Map(string fieldName, string propertyName)
        {
            _typeConfiguration.Add(fieldName, propertyName);
            return this;
        }
    }
}
