namespace Serialization.MicrosoftJson.Exceptions
{
    using System;
    using System.Reflection;

    public class MissingGetMethodException : MissingMemberException
    {
        public MissingGetMethodException(PropertyInfo propertyInfo) : base($"Get method missing on property '{propertyInfo.Name}'")
            => PropertyInfo = propertyInfo;

        public PropertyInfo PropertyInfo { get; }
    }
}
