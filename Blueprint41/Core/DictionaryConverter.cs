﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blueprint41;

public class DictionaryConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType)
        {
            return false;
        }

        if (typeToConvert.GetGenericTypeDefinition() != typeof(Dictionary<,>))
        {
            return false;
        }

        return true;
    }

    public override JsonConverter CreateConverter(
        Type type,
        JsonSerializerOptions options)
    {
        Type keyType = type.GetGenericArguments()[0];
        Type valueType = type.GetGenericArguments()[1];
        var obj = Activator.CreateInstance(
            typeof(DictionaryConverterInner<,>).MakeGenericType(keyType, valueType),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: [options],
            culture: null);
        var converter = obj as JsonConverter;

        return converter!;
    }

    private class DictionaryConverterInner<TKey, TValue>(JsonSerializerOptions options) :
        JsonConverter<Dictionary<TKey, TValue>>
    {
        private readonly JsonConverter<TKey> _keyConverter = (JsonConverter<TKey>)options
                .GetConverter(typeof(TKey));
        private readonly JsonConverter<TValue> _valueConverter = (JsonConverter<TValue>)options
                .GetConverter(typeof(TValue));
        private readonly Type _keyType = typeof(TKey);
        private readonly Type _valueType = typeof(TValue);

        public override Dictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {

            if (reader.TokenType != JsonTokenType.StartObject && reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            bool simpleDictionary = reader.TokenType != JsonTokenType.StartArray;
            var dictionary = new Dictionary<TKey, TValue>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject || reader.TokenType == JsonTokenType.EndArray)
                {
                    return dictionary;
                }

                TKey? key;
                TValue? value;
                if (simpleDictionary)
                    reader = ParseSimple(reader, options, out key, out value);
                else
                    reader = ParseComplex(reader, options, out key, out value);

                if (key is null)
                    throw new JsonException("Dictionary entry should have a key.");
                else
#pragma warning disable CS8604 // Possible null reference argument.
                    dictionary.Add(key, value ?? default);
#pragma warning restore CS8604 // Possible null reference argument.
            }

            throw new JsonException();
        }

        private Utf8JsonReader ParseSimple(Utf8JsonReader reader, JsonSerializerOptions options, out TKey? key, out TValue? value)
        {
            // Get the key.
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            key = default(TKey);
            value = default(TValue);

            if (_keyConverter != null)
            {
                key = _keyConverter.Read(ref reader, _keyType, options);
            }
            else
            {
                key = JsonSerializer.Deserialize<TKey>(ref reader, options);
            }
            if (_valueConverter != null)
            {
                reader.Read();
                if (reader.TokenType != JsonTokenType.Null)
                    value = _valueConverter.Read(ref reader, _valueType, options);
            }
            else
            {
                if (reader.TokenType != JsonTokenType.Null)
                    value = JsonSerializer.Deserialize<TValue>(ref reader, options);
            }

            return reader;
        }
        private Utf8JsonReader ParseComplex(Utf8JsonReader reader, JsonSerializerOptions options, out TKey? key, out TValue? value)
        {
            // Get the key.
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expects start of object");

            bool keyIsAssigned = false;
            bool valueIsAssigned = false;
            key = default(TKey);
            value = default(TValue);
            while (!keyIsAssigned || !valueIsAssigned)
            {
                reader.Read();

                // Get the key.
                if (reader.TokenType != JsonTokenType.PropertyName && (reader.GetString()?.ToLower() != "key" || reader.GetString()?.ToLower() != "value"))
                    throw new JsonException();

                if (reader.GetString()!.ToLower() == "key")
                {
                    if (_keyConverter != null)
                    {
                        reader.Read();
                        key = _keyConverter.Read(ref reader, _keyType, options);
                    }
                    else
                    {
                        key = JsonSerializer.Deserialize<TKey>(ref reader, options);
                    }
                    keyIsAssigned = true;
                }
                else if (reader.GetString()!.ToLower() == "value")
                {
                    if (_valueConverter != null)
                    {
                        reader.Read();
                        if (reader.TokenType != JsonTokenType.Null)
                            value = _valueConverter.Read(ref reader, _valueType, options);
                    }
                    else
                    {
                        if (reader.TokenType != JsonTokenType.Null)
                            value = JsonSerializer.Deserialize<TValue>(ref reader, options);
                    }
                    valueIsAssigned = true;
                }
                else
                    throw new JsonException();
            }

            if (!keyIsAssigned || !valueIsAssigned)
                throw new JsonException("Dictionary entry should have a key and a value.");

            reader.Read();
            // Get the key.
            if (reader.TokenType != JsonTokenType.EndObject)
                throw new JsonException("Expects end of object");

            return reader;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<TKey, TValue> dictionary, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (KeyValuePair<TKey, TValue> kvp in dictionary)
            {
                var propertyName = kvp.Key!.ToString() ?? string.Empty;
                writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(propertyName) ?? propertyName);

                if (_valueConverter != null)
                {
                    if (kvp.Value is not null)
                        _valueConverter.Write(writer, kvp.Value, options);
                    else
                        writer.WriteNullValue();
                }
                else
                {
                    if (kvp.Value is not null)
                        JsonSerializer.Serialize(writer, kvp.Value, options);
                    else
                        writer.WriteNullValue();
                }
            }

            writer.WriteEndObject();
        }
    }
}
