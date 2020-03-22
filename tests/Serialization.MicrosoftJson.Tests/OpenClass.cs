namespace Serialization.MicrosoftJson.Tests
{
    public class OpenClass<TValue>
    {
        public OpenClass() { }
        
        public OpenClass(TValue value) => Value = value;

        public TValue Value { get; set; }
    }
}
