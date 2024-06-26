﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Blueprint41.Neo4j.Model;

namespace Blueprint41.Query;

public abstract partial class ListResult<TList, TResult> : AliasResult
    where TList : ListResult<TList, TResult>, IAliasListResult
    where TResult : AliasResult, IAliasResult
{
    protected ListResult(Func<QueryTranslator, string?>? function, object[]? arguments, Type? type) : base(function, arguments, type) { }
    protected ListResult(FieldResult? parent, Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? type = null) : base(parent, function, arguments, type) { }
    protected ListResult(AliasResult parent, Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? type = null) : base(parent, function, arguments, type) { }

    public TResult this[int index]
    {
        get
        {
            return NewResult(t => t.FnListGetItem, [Parameter.Constant<int>(index)]);
        }
    }
    public TResult this[NumericResult index]
    {
        get
        {
            return NewResult(t => t.FnListGetItem, [index]);
        }
    }

    public TResult Head() => NewResult(t => t.FnListHead);
    public TResult Last() => NewResult(t => t.FnListLast);
    public NumericResult Size() => new(this, t => t.FnSize, null, typeof(long));
    public TList Sort(Func<TResult, string> fieldName, bool ascending)
    {
        string fld = fieldName.Invoke((TResult)this.Alias!);
        ResultHelper<TList> ofLst = ResultHelper.Of<TList>();
        Literal[] literals = ascending ? [new Literal(string.Concat("^", fld))] : [new Literal(fld)];
        var f = (QueryTranslator t) => t.FnApocCollSortNodes;
        return ofLst.NewAliasResult(this, f, literals!);
    }

    public TList Union(TList list) => NewList(t => t.FnApocCollUnion, [list]);
    public TList UnionAll(TList list) => NewList(t => t.FnApocCollUnionAll, [list]);

    public TList Filter(Func<TResult, QueryCondition> condition)
    {
        TResult conditionItemField = NewResult(t => "item", []);
        QueryCondition test = condition.Invoke(conditionItemField);

        return NewList(t => t.FnListSelectWhere, [test]);
    }

    //public QueryCondition All(Func<TResult, QueryCondition> condition)
    //{
    //    TResult alias = NewResult("item", new object[0]);
    //    QueryCondition test = condition.Invoke(alias);

    //    return new QueryCondition(new BooleanResult("all(item IN {base} WHERE {0})", new object[] { test }));
    //}
    //public QueryCondition Any(Func<TResult, QueryCondition> condition)
    //{
    //    TResult alias = NewResult("item", new object[0]);
    //    QueryCondition test = condition.Invoke(itemField);

    //    return new QueryCondition(new BooleanResult(itemField, "any(item IN {base} WHERE {0})", new object[] { test }));
    //}
    //public QueryCondition None(Func<TResult, QueryCondition> condition)
    //{
    //    TResult alias = NewResult("item", new object[0]);
    //    QueryCondition test = condition.Invoke(itemField);

    //    return new QueryCondition(new BooleanResult(this, "none(item IN {base} WHERE {0})", new object[] { test }));
    //}
    //public QueryCondition Single(Func<TResult, QueryCondition> condition)
    //{
    //    TResult alias = NewResult("item", new object[0]);
    //    QueryCondition test = condition.Invoke(itemField);

    //    return new QueryCondition(new BooleanResult(this, "single(item IN {base} WHERE {0})", new object[] { test }));
    //}

    public AsResult As(string aliasName, out TList alias)
    {
        AliasResult aliasResult = new(this, null)
        {
            AliasName = aliasName
        };

        alias = ResultHelper.Of<TList>().NewAliasResult(aliasResult, null, null, null);
        return this.As(aliasName);
    }

    public override string GetFieldName() => throw new NotSupportedException();

    protected internal TResult NewResult(Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? type = null) => ResultHelper.Of<TResult>().NewAliasResult(this, function, arguments, type);
    protected internal TList NewList(Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? type = null) => ResultHelper.Of<TList>().NewAliasResult(this, function, arguments, type);
}
public abstract partial class ListResult<TList, TResult, TType> : FieldResult
    where TList : ListResult<TList, TResult, TType>, IPrimitiveListResult
    where TResult : FieldResult, IPrimitiveResult
{
    protected ListResult(Func<QueryTranslator, string?>? function, object[]? arguments, Type? type) : base(function, arguments, type) { }
    protected ListResult(FieldResult? parent, Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? type = null) : base(parent, function, arguments, type) { }
    protected ListResult(AliasResult alias, Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? type = null) : base(alias, function, arguments, type) { }
    protected ListResult(AliasResult alias, string? fieldName, IEntity? entity, Property? property, Type? overridenReturnType = null) : base(alias, fieldName, entity, property, overridenReturnType) { }

    public TResult this[int index]
    {
        get
        {
            return NewResult(t => t.FnListGetItem, [Parameter.Constant<int>(index)]);
        }
    }
    public TResult this[NumericResult index]
    {
        get
        {
            return NewResult(t => t.FnListGetItem, [index]);
        }
    }

    public TResult Head() => NewResult(t => t.FnListHead);
    public TResult Last() => NewResult(t => t.FnListLast);
    public NumericResult Size() => new(this, t => t.FnSize, null, typeof(long));
    public TList Sort() => NewList(t => t.FnApocCollSort);
    public TList Flatten() => NewList(t => t.FnApocCollFlatten);
    public TList Collect() => NewList(t => t.FnCollect);
    public TList CollectDistinct() => NewList(t => t.FnCollectDistinct);
    public override NumericResult Count() => new(this, t => t.FnListSize, null, typeof(long));

    public TList Union(TList list) => NewList(t => t.FnApocCollUnion, [list]);
    public TList UnionAll(TList list) => NewList(t => t.FnApocCollUnionAll, [list]);

    public StringResult Reduce(string init, Func<StringResult, TResult, StringResult> logic)
    {
        StringResult valueField = new(t => "value", [], typeof(string));
        TResult itemField = NewResult(t => "item", [], typeof(TType));
        StringResult result = logic.Invoke(valueField, itemField);

        return new StringResult(this, t => t.FnListReduce, [Parameter.Constant<string>(init), result], typeof(string));
    }
    public TList Select(Func<TResult, StringResult> logic, Func<TResult, QueryCondition>? condition = null)
    {
        TResult itemField = NewResult(t => "item", [], typeof(TType));
        StringResult result = logic.Invoke(itemField);

        if (condition is not null)
        {
            TResult conditionItemField = NewResult(t => "item", [], typeof(TType));
            QueryCondition test = condition.Invoke(conditionItemField);

            return NewList(t => t.FnListSelectValueWhere, [test, result], typeof(TType));
        }
        else
            return NewList(t => t.FnListSelect, [result], typeof(TType));
    }
    public TList Filter(Func<TResult, QueryCondition> condition)
    {
        TResult conditionItemField = NewResult(t => "item", [], typeof(TType));
        QueryCondition test = condition.Invoke(conditionItemField);

        return NewList(t => t.FnListSelectWhere, [test], typeof(TType));
    }

    public IListResult Select<TValue>(Func<TResult, TValue> logic, Func<TResult, QueryCondition>? condition = null)
         where TValue : IResult
    {
        ResultHelper info = ResultHelper.Of<TValue>();
        ResultHelper listInfo = info.ListType ?? throw new NotSupportedException("Select on a jagged-list is not supported.");

        TResult itemField = NewResult(t => "item", [], typeof(TType));
        TValue result = logic.Invoke(itemField);

        if (condition is not null)
        {
            TResult conditionItemField = NewResult(t => "item", [], typeof(TType));
            QueryCondition test = condition.Invoke(conditionItemField);

            return NewList(t => t.FnListSelectValueWhere, result, [test, result], typeof(TType));
        }
        else
            return NewList(t => t.FnListSelect, result, [result], typeof(TType));

        IListResult NewList(Func<QueryTranslator, string?>? function, TValue result, object[]? arguments = null, Type? type = null)
        {
            var listResult = (IListResult)listInfo.NewFieldResult((IPrimitiveResult)this, function, arguments, type);
            if (result is AliasResult aliasResult)
            {
                if (listResult is AliasResult listAliasResult)
                    listAliasResult.Node = aliasResult.Node;
            }
            return listResult;
        }
    }

    public StringResult Join(string separator, Func<TResult, StringResult> logic)
    {
        TResult itemField = NewResult(t => "item", [], typeof(TType));
        StringResult result = logic.Invoke(itemField);

        return Reduce("", (value, item) => value.Concat(Parameter.Constant(separator), result).Substring(separator.Length));
    }
    public QueryCondition All(TType value) => All(item => item == Parameter.Constant(value));
    public QueryCondition All(Func<TResult, QueryCondition> condition)
    {
        TResult itemField = NewResult(t => "item", [], typeof(TType));
        QueryCondition test = condition.Invoke(itemField);

        return new QueryCondition(new BooleanResult(this, t => t.FnListAll, [test]));
    }
    public QueryCondition Any(TType value) => Any(item => item == Parameter.Constant(value));
    public QueryCondition Any(Parameter parameter) => Any(item => item == parameter);
    public QueryCondition Any(Func<TResult, QueryCondition> condition)
    {
        TResult itemField = NewResult(t => "item", [], typeof(TType));
        QueryCondition test = condition.Invoke(itemField);

        return new QueryCondition(new BooleanResult(this, t => t.FnListAny, [test]));
    }
    public QueryCondition None(TType value) => None(item => item == Parameter.Constant(value));
    public QueryCondition None(Func<TResult, QueryCondition> condition)
    {
        TResult itemField = NewResult(t => "item", [], typeof(TType));
        QueryCondition test = condition.Invoke(itemField);

        return new QueryCondition(new BooleanResult(this, t => t.FnListNone, [test]));
    }
    public QueryCondition Single(TType value) => Single(item => item == Parameter.Constant(value));
    public QueryCondition Single(Func<TResult, QueryCondition> condition)
    {
        TResult itemField = NewResult(t => "item", [], typeof(TType));
        QueryCondition test = condition.Invoke(itemField);

        return new QueryCondition(new BooleanResult(itemField, t => t.FnListSingle, [test]));
    }

    protected internal TResult NewResult(Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? type = null) => ResultHelper.Of<TResult>().NewFieldResult((IPrimitiveResult)this, function, arguments, type ?? typeof(TType));
    protected internal TList NewList(Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? type = null) => ResultHelper.Of<TList>().NewFieldResult((IPrimitiveResult)this, function, arguments, type);
}
