namespace Serialization.MicrosoftJson.Tests
{
    public sealed class ClosedClass<TValue>
    {
        private readonly TValue _value;

        public ClosedClass(TValue value) => _value = value;

        public TValue Value => _value;
    }
}
