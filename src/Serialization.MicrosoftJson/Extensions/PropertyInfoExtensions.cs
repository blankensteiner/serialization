namespace Serialization.MicrosoftJson.Extensions
{
    using Serialization.MicrosoftJson.Exceptions;
    using System.Reflection;

    public static class PropertyInfoExtensions
    {
        public static MethodInfo GetRequiredGetMethod(this PropertyInfo propertyInfo)
        {
            var methodInfo = propertyInfo.GetGetMethod();

            if (methodInfo is null)
                throw new MissingGetMethodException(propertyInfo);

            return methodInfo;
        }
    }
}
