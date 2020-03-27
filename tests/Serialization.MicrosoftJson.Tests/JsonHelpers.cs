namespace Serialization.MicrosoftJson.Tests
{
    using System;
    using System.Text.Json;

    public static class JsonHelpers
    {
        private static JsonElement GetProperty(JsonDocument jsonDocument, string propertyName)
        {
            var element = jsonDocument.RootElement;
            foreach (var name in propertyName.Split('.'))
            {
                element = element.GetProperty(name);
            }
            return element;
        }

        public static bool GetBoolean(byte[] bytes, string propertyName)
        {
            using var jsonDocument = JsonDocument.Parse(bytes);
            return GetProperty(jsonDocument, propertyName).GetBoolean();
        }

        public static byte GetInt8(byte[] bytes, string propertyName)
        {
            using var jsonDocument = JsonDocument.Parse(bytes);
            return GetProperty(jsonDocument, propertyName).GetByte();
        }

        public static sbyte GetUInt8(byte[] bytes, string propertyName)
        {
            using var jsonDocument = JsonDocument.Parse(bytes);
            return GetProperty(jsonDocument, propertyName).GetSByte();
        }

        public static short GetInt16(byte[] bytes, string propertyName)
        {
            using var jsonDocument = JsonDocument.Parse(bytes);
            return GetProperty(jsonDocument, propertyName).GetInt16();
        }

        public static ushort GetUInt16(byte[] bytes, string propertyName)
        {
            using var jsonDocument = JsonDocument.Parse(bytes);
            return GetProperty(jsonDocument, propertyName).GetUInt16();
        }

        public static int GetInt32(byte[] bytes, string propertyName)
        {
            using var jsonDocument = JsonDocument.Parse(bytes);
            return GetProperty(jsonDocument, propertyName).GetInt32();
        }

        public static uint GetUInt32(byte[] bytes, string propertyName)
        {
            using var jsonDocument = JsonDocument.Parse(bytes);
            return GetProperty(jsonDocument, propertyName).GetUInt32();
        }

        public static long GetInt64(byte[] bytes, string propertyName)
        {
            using var jsonDocument = JsonDocument.Parse(bytes);
            return GetProperty(jsonDocument, propertyName).GetInt64();
        }

        public static ulong GetUInt64(byte[] bytes, string propertyName)
        {
            using var jsonDocument = JsonDocument.Parse(bytes);
            return GetProperty(jsonDocument, propertyName).GetUInt64();
        }

        public static float GetSingle(byte[] bytes, string propertyName)
        {
            using var jsonDocument = JsonDocument.Parse(bytes);
            return GetProperty(jsonDocument, propertyName).GetSingle();
        }

        public static double GetDouble(byte[] bytes, string propertyName)
        {
            using var jsonDocument = JsonDocument.Parse(bytes);
            return GetProperty(jsonDocument, propertyName).GetDouble();
        }

        public static decimal GetDecimal(byte[] bytes, string propertyName)
        {
            using var jsonDocument = JsonDocument.Parse(bytes);
            return GetProperty(jsonDocument, propertyName).GetDecimal();
        }

        public static string GetString(byte[] bytes, string propertyName)
        {
            using var jsonDocument = JsonDocument.Parse(bytes);
            return GetProperty(jsonDocument, propertyName).GetString();
        }

        public static Guid GetGuid(byte[] bytes, string propertyName)
        {
            using var jsonDocument = JsonDocument.Parse(bytes);
            return GetProperty(jsonDocument, propertyName).GetGuid();
        }

        public static DateTime GetDateTime(byte[] bytes, string propertyName)
        {
            using var jsonDocument = JsonDocument.Parse(bytes);
            return GetProperty(jsonDocument, propertyName).GetDateTime();
        }

        public static DateTimeOffset GetDateTimeOffset(byte[] bytes, string propertyName)
        {
            using var jsonDocument = JsonDocument.Parse(bytes);
            return GetProperty(jsonDocument, propertyName).GetDateTimeOffset();
        }

        public static bool PropertyExists(byte[] bytes, string propertyName)
        {
            using var jsonDocument = JsonDocument.Parse(bytes);
            return jsonDocument.RootElement.TryGetProperty(propertyName, out JsonElement _);
        }
    }
}
