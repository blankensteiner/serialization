namespace Serialization.MicrosoftJson.Extensions
{
    using System;
    using System.Reflection;

    public static class FieldInfoExtensions
    {
        public static Type GetUnderlyingType(this FieldInfo fieldInfo)
        {
            var underlyingType = Nullable.GetUnderlyingType(fieldInfo.FieldType);

            if (underlyingType is null)
                underlyingType = fieldInfo.FieldType;

            if (underlyingType.IsEnum)
                underlyingType = Enum.GetUnderlyingType(underlyingType);

            return underlyingType;
        }
    }
}
