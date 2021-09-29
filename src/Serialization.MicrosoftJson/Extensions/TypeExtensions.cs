namespace Serialization.MicrosoftJson.Extensions
{
    using Serialization.MicrosoftJson.Exceptions;
    using System;
    using System.Reflection;

    public static class TypeExtensions
    {
        public static ConstructorInfo GetRequiredConstructor(this Type type, Type[] types)
        {
            var constructorInfo = type.GetConstructor(types);

            if (constructorInfo is null)
                throw new MissingConstructorException(type.FullName, types);

            return constructorInfo;
        }

        public static MethodInfo GetRequiredMethod(this Type type, string name)
            => type.GetRequiredMethod(name, Type.EmptyTypes);

        public static MethodInfo GetRequiredMethod(this Type type, string name, Type[] types)
        {
            var methodInfo = type.GetMethod(name, types);

            if (methodInfo is null)
                throw new MissingMethodException(type.FullName, name);

            return methodInfo;
        }

        public static PropertyInfo GetRequiredProperty(this Type type, string name)
        {
            var propertyInfo = type.GetProperty(name);

            if (propertyInfo is null)
                throw new MissingPropertyException(type.FullName, name);

            return propertyInfo;
        }
    }
}
