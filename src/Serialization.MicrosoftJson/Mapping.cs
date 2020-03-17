namespace Serialization.MicrosoftJson
{
    using System.Reflection;

    public sealed class Mapping
    {
        public Mapping(FieldInfo field)
        {
            Field = field;
        }

        public string? PropertyName { get; set; }
        public FieldInfo Field { get; }
    }
}
