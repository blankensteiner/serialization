namespace Serialization.MicrosoftJson.Exceptions
{
    using System;

    public class MissingConstructorException : MissingMemberException
    {
        public MissingConstructorException(string? className, Type[] types) : base(className, "ctor")
            => Parameters = types;

        public Type[] Parameters { get; }
    }
}
