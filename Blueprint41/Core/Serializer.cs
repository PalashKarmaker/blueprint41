﻿using Blueprint41.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blueprint41;

internal abstract class Serializer
{
    internal static Lazy<JsonSerializerOptions> serializeOptions = new(
        delegate () 
        {
            JsonSerializerOptions option = new();
            option.Converters.Add(new DictionaryConverter());
            return option;
        }, 
        false
    );
        
    protected abstract string SerializeInternal(object? value);
    protected abstract object? DeserializeInternal(string value);

    public static string Serialize(object? value) => GetSerializer(value?.GetType())!.SerializeInternal(value);
    public static T Deserialize<T>(string value) => (T)GetSerializer(typeof(T))!.DeserializeInternal(value)!;

    private static Serializer? GetSerializer(Type? type)
    {
        type ??= typeof(object);

        return cache.TryGetOrAdd(type, key =>
        {
            Type serializerType = typeof(Serializer<>).MakeGenericType([type]);
            return (Serializer)Activator.CreateInstance(serializerType)!;
        });
    }
    private static AtomicDictionary<Type, Serializer> cache = [];
}
internal class Serializer<T> : Serializer
{
    private static DataContractJsonSerializer serializer = new(typeof(T));

    protected sealed override string SerializeInternal(object? value) => Serializer<T>.Serialize((T)value!);
    protected sealed override object? DeserializeInternal(string value) => Serializer<T>.Deserialize(value);

    public static string Serialize(T value)
    {
        using MemoryStream ms = new();
        JsonSerializer.Serialize(ms, value); //Serialize with default converter
        ms.Position = 0;
        using (StreamReader sr = new(ms, Encoding.UTF8))
        {
            return sr.ReadToEnd();
            //var retval = sr.ReadToEnd();
            //if (retval.Contains("Certus_Configuration"))
            //    System.Diagnostics.Debugger.Break();
            //return retval;
        }
    }
    public static T Deserialize(string value)
    {
        using MemoryStream ms = new(Encoding.UTF8.GetBytes(value));
        if (value.TrimStart().StartsWith("["))
            return (T)serializer.ReadObject(ms)!;
        else
            return JsonSerializer.Deserialize<T>(ms)!;
    }
}
