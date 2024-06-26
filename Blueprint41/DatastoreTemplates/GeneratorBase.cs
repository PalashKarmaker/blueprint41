﻿using System;
using System.Collections.Generic;
using System.Text;

using Blueprint41.Core;
using Blueprint41.Dynamic;
using Blueprint41.Neo4j.Persistence.Void;

namespace Blueprint41.DatastoreTemplates;

public abstract partial class GeneratorBase
{
    public abstract string TransformText();
    public Entity? DALModel { get; set; }
    public Relationship? DALRelation { get; set; }
    public DatastoreModel? Datastore { get; set; }
    public IReadOnlyList<TypeMapping> SupportedTypeMappings => Datastore?.PersistenceProvider?.SupportedTypeMappings ?? Neo4jPersistenceProvider.VoidPersistenceProvider.SupportedTypeMappings;

    public GeneratorSettings? Settings { get; set; }

    public static void Log(string text, params object[] arguments)
    {

    }

    public GeneratorBase()
    {
    }

    public class MappingIssue
    {
        internal MappingIssue(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; private set; }
        public Type? ReturnType { get; set; }
        public Type? DatastoreType { get; set; }
        public bool IsNullable { get; set; }
        public bool MapFrom { get; set; }
        public bool MapTo { get; set; }
    }

    protected struct TypeSafeStaticData
    {
        internal TypeSafeStaticData(string propertyName, DynamicEntity item)
        {
            PropertyName = propertyName;
            KeyValue = item?.GetKey()?.ToString();
            SafeValue = null;
            OriginalValue = null;

            if (item is not null && item.TryGetMember(propertyName, out var value) && value is not null)
            {
                OriginalValue = value.ToString();
                SafeValue = SafeCSharpName(value.ToString() ?? string.Empty);
            }
        }

        public string PropertyName;
        public string? KeyValue;
        public string? SafeValue;
        public string? OriginalValue;
        private string SafeCSharpName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "_";

            StringBuilder safe = new();
            if (name.Length == 0 || NeedsUnderscore(name[0]))
                safe.Append("_");

            foreach (char c in name)
            {
                if (IsSafeChar(c))
                    safe.Append(c);
                else if (c == ' ' || c == '-')
                    safe.Append('_');
                else if (c == '+')
                    safe.Append("_plus_");
            }

            return safe.ToString().Replace("__", "_");
        }
        private static bool NeedsUnderscore(char c) => !(('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z') || (c == '_'));
        private static bool IsSafeChar(char c) => (('0' <= c && c <= '9') || ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z') || (c == '_'));
    }
    protected static string EmitConstantValue(string value, Type type)
    {
        if (type == typeof(string))
            return string.Concat("\"", value, "\"");

        if (type == typeof(int) || type == typeof(short) || type == typeof(ushort) || type == typeof(byte) || type == typeof(sbyte) || type == typeof(bool))
            return value;

        if (type == typeof(uint))
            return string.Concat(value, "U");

        if (type == typeof(long))
            return string.Concat(value, "L");

        if (type == typeof(ulong))
            return string.Concat(value, "UL");

        if (type == typeof(decimal))
            return string.Concat(value, "M");

        if (type == typeof(double))
            return string.Concat(value, "D");

        if (type == typeof(float))
            return string.Concat(value, "F");

        throw new NotSupportedException();
    }

    protected static bool SupportedKeyType(Type type) => (
            type == typeof(string) ||
            type == typeof(int) ||
            type == typeof(short) ||
            type == typeof(ushort) ||
            type == typeof(byte) ||
            type == typeof(sbyte) ||
            type == typeof(bool) ||
            type == typeof(uint) ||
            type == typeof(long) ||
            type == typeof(ulong) ||
            type == typeof(decimal) ||
            type == typeof(double) ||
            type == typeof(float)
        );

    protected static string GetResultType(Type type)
    {
        switch (type.Name)
        {
            case "Boolean":
                return "BooleanResult";
            case "Int16":
            case "Int32":
            case "Int64":
            case "Decimal":
                return "NumericResult";
            case "Single":
            case "Double":
                return "FloatResult";
            case "Guid":
            case "String":
                return "StringResult";
            case "DateTime":
                return "DateTimeResult";
            case "List`1":
                if (type.GenericTypeArguments[0] == typeof(string))
                    return "StringListResult";
                else
                    return "ListResult";
            case "String[]":
                return "StringListResult";
            default:
                if (type.IsEnum)
                    return "StringResult";

                return "MiscResult";
        }
    }
}
