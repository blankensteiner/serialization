namespace Serialization.MicrosoftJson.Exceptions
{
    using System;

    public class MissingPropertyException : MissingMemberException
    {
        public MissingPropertyException(string? className, string propertyName) : base(className, propertyName) { }
    }
}
