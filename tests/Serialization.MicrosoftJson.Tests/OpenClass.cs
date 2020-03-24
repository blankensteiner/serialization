namespace Serialization.MicrosoftJson.Tests
{
    public class OpenClass<TValue>
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public OpenClass() { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public OpenClass(TValue value) => Value = value;

        public TValue Value { get; set; }
    }
}
