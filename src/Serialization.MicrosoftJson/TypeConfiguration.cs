namespace Serialization.MicrosoftJson
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public sealed class TypeConfiguration
    {
        private readonly Dictionary<string, Mapping> _mappings;

        public TypeConfiguration(Type type, int version, bool fallback)
        {
            Type = type;
            Version = version;
            Fallback = fallback;
            _mappings = new Dictionary<string, Mapping>();

            var fields = Type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                _mappings[field.Name] = new Mapping(field);
            }
        }

        public Type Type { get; }
        public int Version { get; }
        public bool Fallback { get; }
        public void Add(string fieldName, string propertyName) => _mappings[fieldName].PropertyName = propertyName;
        public void Remove(string fieldName) => _mappings.Remove(fieldName);
        public IEnumerable<Mapping> Mappings => _mappings.Values;
    }
}
