﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blueprint41.Query;

public static partial class Functions
{
    public static Parameter NULL { get { return null!; } }
    public static StringResult NewGuid() => new(t => t.FnApocCreateUuid, null, typeof(string));
    public static FloatResult Pi() => new(t => t.FnPi, null, typeof(Double));
    public static FloatResult Rand() => new(t => t.FnRand, null, typeof(Double));
    public static StringResult SHA1(params FieldResult[] fields) => new(t => t.FnApocUtilSHA1(fields.Length), fields, typeof(string));
    public static StringResult MD5(params FieldResult[] fields) => new(t => t.FnApocUtilMD5(fields.Length), fields, typeof(string));
    public static NumericListResult Range(int start, int end, int step) => new(t => t.FnRange, [Parameter.Constant(start), Parameter.Constant(end), Parameter.Constant(step)], typeof(int));
    public static NumericListResult Range(int start, int end, NumericResult step) => new(t => t.FnRange, [Parameter.Constant(start), Parameter.Constant(end), step], typeof(int));
    public static NumericListResult Range(int start, NumericResult end, int step) => new(t => t.FnRange, [Parameter.Constant(start), end, Parameter.Constant(step)], typeof(int));
    public static NumericListResult Range(int start, NumericResult end, NumericResult step) => new(t => t.FnRange, [Parameter.Constant(start), end, step], typeof(int));
    public static NumericListResult Range(NumericResult start, int end, int step) => new(t => t.FnRange, [start, Parameter.Constant(end), Parameter.Constant(step)], typeof(int));
    public static NumericListResult Range(NumericResult start, int end, NumericResult step) => new(t => t.FnRange, [start, Parameter.Constant(end), step], typeof(int));
    public static NumericListResult Range(NumericResult start, NumericResult end, int step) => new(t => t.FnRange, [start, end, Parameter.Constant(step)], typeof(int));
    public static NumericListResult Range(NumericResult start, NumericResult end, NumericResult step) => new(t => t.FnRange, [start, end, step], typeof(int));
    public static StringResult NodeType(AliasResult alias)
    {
        if (alias is null)
            throw new ArgumentNullException(nameof(alias));

        if (alias.Node is null)
            throw new ArgumentException("The alias is not bound to an Entity.");

        Parameter concreteLabels = Parameter.Constant(alias.Node.Entity.GetConcreteClasses().Select(y => y.Label.Name).Distinct().ToList());
        return new StringResult(alias, t => "HEAD(apoc.coll.intersection(LABELS({base}), {0}))", [concreteLabels], typeof(string));
    }
    private static StringResult FingerPrint(AliasResult alias, string config)
    {
        if (alias is null)
            throw new ArgumentNullException(nameof(alias));

        if (alias.Node is null)
            throw new ArgumentException("The alias is not bound to an Entity.");

        Parameter concreteLabel = Parameter.Constant(alias.Node.Entity.GetConcreteClasses().First().Label.Name);
        return new StringResult(alias, t => string.Concat("apoc.hashing.fingerprinting({base},", config,")"), [concreteLabel], typeof(string));
    }

    public static StringResult FingerPrint(AliasResult alias) => FingerPrint(alias, "{{}}");

    public static StringResult FingerPrintExcludingFields(AliasResult alias, params FieldResult[] excludedFields)
    {
        if(excludedFields.Length == 0)
            return FingerPrint(alias, "{{}}");

        var fields = string.Join(",", excludedFields.Select(field => string.Concat("\"", field.FieldName, "\"")));

        return FingerPrint(alias, string.Concat("{{ allNodesDisallowList: [", fields, "] }}"));
    }
    public static StringResult FingerPrintWithFields(AliasResult alias, params FieldResult[] includedFields)
    {
        if (includedFields.Length == 0)
            return FingerPrint(alias, "{{}}");

        var fields = string.Join(",", includedFields.Select(field => string.Concat("\"", field.FieldName, "\"")));

        return FingerPrint(alias, string.Concat("{{ allNodesAllowList: [", fields, "] }}"));
    }

    public static T CaseWhen<T>(QueryCondition condition, Parameter @then, Parameter @else)
         where T : IPlainPrimitiveResult
    {
        ResultHelper<T> info = ResultHelper.Of<T>();
        Type underlyingType = info.UnderlyingType!;

        return ResultHelper<T>.NewFunctionResult(t => t.FnCaseWhen, [condition, WrapIfNull(@then), WrapIfNull(@else)], underlyingType);
    }
    public static T CaseWhen<T>(QueryCondition condition, Parameter @then, T @else)
         where T : IPlainPrimitiveResult
    {
        ResultHelper<T> info = ResultHelper.Of<T>();
        Type underlyingType = info.UnderlyingType!;

        return ResultHelper<T>.NewFunctionResult(t => t.FnCaseWhen, [condition, WrapIfNull(@then), WrapIfNull(@else)], underlyingType);
    }
    public static T CaseWhen<T>(QueryCondition condition, T @then, Parameter @else)
         where T : IPlainPrimitiveResult
    {
        ResultHelper<T> info = ResultHelper.Of<T>();
        Type underlyingType = info.UnderlyingType!;

        return ResultHelper<T>.NewFunctionResult(t => t.FnCaseWhen, [condition, WrapIfNull(@then), WrapIfNull(@else)], underlyingType);
    }
    public static T CaseWhen<T>(QueryCondition[] conditions, Parameter[] thens, Parameter @else)
         where T : IPlainPrimitiveResult
    {
        if (conditions.Length != thens.Length)
            throw new ArgumentException("The number of condition and then statement should match.");

        ResultHelper<T> info = ResultHelper.Of<T>();
        Type underlyingType = info.UnderlyingType!;

        object[] arguments = new object[(conditions.Length << 1) + 1];
        int argNum = 0;
        for (int index = 0; index < conditions.Length; index++)
        {
            arguments[argNum] = conditions[index];
            argNum++;

            arguments[argNum] = WrapIfNull(thens[index]);
            argNum++;
        }
        arguments[argNum] = WrapIfNull(@else);

        return ResultHelper<T>.NewFunctionResult(t => t.FnCaseWhenMultiple(conditions.Length), arguments, underlyingType);
    }
    public static T CaseWhen<T>(QueryCondition condition, T @then, T @else)
        where T : IResult
    {
        ResultHelper<T> info = ResultHelper.Of<T>();
        Type? underlyingType = info.UnderlyingType;

        return ResultHelper<T>.NewFunctionResult(t => t.FnCaseWhen, [condition, WrapIfNull(@then), WrapIfNull(@else)], underlyingType);
    }
    public static T CaseWhen<T>(QueryCondition[] conditions, T[] thens, T @else)
         where T : IResult
    {
        if (conditions.Length != thens.Length)
            throw new ArgumentException("The number of condition and then statement should match.");

        ResultHelper<T> info = ResultHelper.Of<T>();
        Type? underlyingType = info.UnderlyingType;

        object[] arguments = new object[(conditions.Length << 1) + 1];
        int argNum = 0;
        for (int index = 0; index < conditions.Length; index++)
        {
            arguments[argNum] = conditions[index];
            argNum++;

            arguments[argNum] = WrapIfNull(thens[index]);
            argNum++;
        }
        arguments[argNum] = WrapIfNull(@else);

        return ResultHelper<T>.NewFunctionResult(t => t.FnCaseWhenMultiple(conditions.Length), arguments, underlyingType);
    }
    private static object WrapIfNull(object value)
    {
        if (value is null)
            return Parameter.Null;

        return value;
    }

    public static StringResult ToUpperCase(Parameter value) => new(t => t.FnToUpper.Replace("base", "0"), [value], value.Type);
    public static StringResult ToLowerCase(Parameter value) => new(t => t.FnToLower.Replace("base", "0"), [value], value.Type);

    public static BooleanResult Exists(QueryCondition pattern) => new(t => t.FnPatternExists, new[] { pattern }, typeof(bool));
    public static BooleanResult Not(BooleanResult field) => new(field, t => t.FnNot, null, typeof(bool));

    public static StringResult Literal(string value)
    {
        object? parameter = value is null ? null : Parameter.Constant(value);
        return new StringResult(t => "{0}", [parameter!], typeof(string));
    }
    public static BooleanResult Literal(bool? value)
    {
        object? parameter = value is null ? null : Parameter.Constant(value);
        return new BooleanResult(t => "{0}", [parameter!], typeof(bool));
    }
    public static DateTimeResult Literal(DateTime? value)
    {
        object? parameter = value is null ? null : Parameter.Constant(value);
        return new DateTimeResult(t => "{0}", [parameter!], typeof(DateTime));
    }
    public static NumericResult Literal(int? value)
    {
        object? parameter = value is null ? null : Parameter.Constant(value);
        return new NumericResult(t => "{0}", [parameter!], typeof(int));
    }
    public static BooleanResult ExistsSubquery(QueryCondition pattern) => new(t => t.FnExistsSubquery, new[] { pattern }, typeof(bool));
    public static BooleanResult ExistsSubquery(Func<Query, ISemiBlankQuery> pattern, IBlankQuery query)
    {
        ((Query)query).SubQueryPart = (Query)pattern.Invoke((Query)query);
        return new BooleanResult(t => t.FnExistsSubquery, new[] { query }, typeof(bool));
    }
    public static NumericResult CountSubquery(QueryCondition pattern) => new(t => t.FnCountSubquery, new[] { pattern }, typeof(int));
    public static NumericResult CountSubquery(Func<Query, ISemiBlankQuery> pattern, IBlankQuery query)
    {
        ((Query)query).SubQueryPart = (Query)pattern.Invoke((Query)query);
        return new NumericResult(t => t.FnCountSubquery, new[] { query }, typeof(int));
    }
    public static TList CollectSubquery<TList>(Func<Query, IReturnQuery> pattern, IBlankQuery query)
         where TList : IListResult
    {
        ResultHelper listInfo = ResultHelper.Of<TList>() ?? throw new NotSupportedException("Collect subquery on a jagged-list is not supported.");
        Type? underlyingType = listInfo.UnderlyingType;
        ((Query)query).SubQueryPart = (Query)pattern.Invoke((Query)query);
        return (TList)listInfo.NewFunctionResult(t => t.FnCollectSubquery, [query], underlyingType);
    }

}
