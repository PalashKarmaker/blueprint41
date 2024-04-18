using Blueprint41.Neo4j.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint41.Query;

public abstract class FieldResult : Result
{
    public static QueryCondition operator ==(FieldResult a, FieldResult b)
    {
        return new QueryCondition(a, Operator.Equals, b);
    }
    public static QueryCondition operator ==(FieldResult a, Parameter b)
    {
        return new QueryCondition(a, Operator.Equals, b);
    }
    public static QueryCondition operator ==(FieldResult left, DateTime right)
    {
        return new QueryCondition(left, Operator.Equals, Parameter.Constant(right));
    }
    public static QueryCondition operator ==(FieldResult left, DateTime? right)
    {
        return new QueryCondition(left, Operator.Equals, Parameter.Constant(right));
    }
    public static QueryCondition operator !=(FieldResult a, FieldResult b)
    {
        return new QueryCondition(a, Operator.NotEquals, b);
    }
    public static QueryCondition operator !=(FieldResult a, Parameter b)
    {
        return new QueryCondition(a, Operator.NotEquals, b);
    }
    public static QueryCondition operator !=(FieldResult left, DateTime right)
    {
        return new QueryCondition(left, Operator.NotEquals, Parameter.Constant(right));
    }
    public static QueryCondition operator !=(FieldResult left, DateTime? right)
    {
        return new QueryCondition(left, Operator.NotEquals, Parameter.Constant(right));
    }
    public static QueryCondition operator >=(FieldResult left, FieldResult right)
    {
        return new QueryCondition(left, Operator.GreaterOrEqual, right);
    }
    public static QueryCondition operator >=(FieldResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.GreaterOrEqual, right);
    }
    public static QueryCondition operator >=(FieldResult left, DateTime? right)
    {
        return new QueryCondition(left, Operator.GreaterOrEqual, Parameter.Constant(right));
    }
    public static QueryCondition operator <=(FieldResult left, FieldResult right)
    {
        return new QueryCondition(left, Operator.LessOrEqual, right);
    }
    public static QueryCondition operator <=(FieldResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.LessOrEqual, right);
    }
    public static QueryCondition operator <=(FieldResult left, DateTime? right)
    {
        return new QueryCondition(left, Operator.LessOrEqual, Parameter.Constant(right));
    }
    public static QueryCondition operator >(FieldResult left, FieldResult right)
    {
        return new QueryCondition(left, Operator.Greater, right);
    }
    public static QueryCondition operator >(FieldResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.Greater, right);
    }
    public static QueryCondition operator >(FieldResult left, DateTime? right)
    {
        return new QueryCondition(left, Operator.Greater, Parameter.Constant(right));
    }
    public static QueryCondition operator <(FieldResult left, FieldResult right)
    {
        return new QueryCondition(left, Operator.Less, right);
    }
    public static QueryCondition operator <(FieldResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.Less, right);
    }
    public static QueryCondition operator <(FieldResult left, DateTime? right)
    {
        return new QueryCondition(left, Operator.Less, Parameter.Constant(right));
    }

    public override bool Equals(object? obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();


    private object[] emptyArguments = [];
    private FieldResult(AliasResult? alias, string? fieldName, IEntity? entity, Property? property, FieldResult? field, Func<QueryTranslator, string?>? function, object[]? arguments, Type? type)
    {
        Alias = alias;
        FieldName = fieldName;
        Entity = entity;
        Property = property;
        Field = field;
        FunctionText = function ?? delegate(QueryTranslator t) { return null; } ;
        FunctionArgs = arguments ?? emptyArguments;
        OverridenReturnType = type;
    }
    protected FieldResult(Func<QueryTranslator, string?>? function, object[]? arguments, Type? type) : this(null, null, null, null, null, function, arguments, type) { }
    protected FieldResult(AliasResult alias, string? fieldName, IEntity? entity, Property? property, Type? overridenReturnType = null) : this(alias, fieldName, entity, property, null, null, null, null) { OverridenReturnType = overridenReturnType; }
    protected FieldResult(AliasResult alias, Func<QueryTranslator, string?>? function, object[]? arguments, Type? type) : this(alias, null, null, null, null, function, arguments, type) { }
    protected FieldResult(FieldResult? field, Func<QueryTranslator, string?>? function, object[]? arguments, Type? type) : this(field?.Alias, field?.FieldName, field?.Entity, field?.Property, field, function, arguments, type) { }
    protected FieldResult(FieldResult field)
    {
        Alias = field.Alias;
        FieldName = field.FieldName;
        Entity = field.Entity;
        Property = field.Property;
        Field = field.Field;
        FunctionText = field.FunctionText;
        FunctionArgs = field.FunctionArgs;
        OverridenReturnType = field.OverridenReturnType;
    }

    public AliasResult? Alias { get; private set; }
    public string? FieldName { get; private set; }
    internal IEntity? Entity { get; private set; }
    internal Property? Property { get; private set; }
    public FieldResult? Field { get; private set; }
    internal Func<QueryTranslator, string?> FunctionText { get; private set; }
    internal object[]? FunctionArgs { get; private set; }
    private Type? OverridenReturnType { get; set; }

    public virtual NumericResult Count() => new(this, t => t.FnCount, null, typeof(long));
    public virtual NumericResult CountDistinct() => new(this, t => t.FnCountDistinct, null, typeof(long));
    public virtual BooleanResult Exists() => new(this, t => t.FnExists, null, typeof(bool));
    public virtual BooleanResult IsNaN() => new(this, t => t.FnIsNaN, null, typeof(bool));
    public virtual BooleanResult ToBoolean() => new(this, t => t.FnToBoolean, null, typeof(bool));

    [Obsolete("Please use the 'ToInteger' method instead.")]
    public virtual NumericResult ToInt() => ToInteger();
    public virtual NumericResult ToInteger() => new(this, t => t.FnToInteger, null, typeof(long));
    public virtual FloatResult ToFloat() => new(this, t => t.FnToFloat, null, typeof(Double));
    new public virtual StringResult ToString() => new(this, t => t.FnToString, null, typeof(string));
    public virtual BooleanListResult ToBooleanList() => new(this, t => t.FnAsIs, null, typeof(bool));
    public virtual NumericListResult ToIntegerList() => new(this, t => t.FnAsIs, null, typeof(long));
    public virtual FloatListResult ToFloatList() => new(this, t => t.FnAsIs, null, typeof(Double));
    public virtual StringListResult ToStringList() => new(this, t => t.FnAsIs, null, typeof(string));

    protected internal override void Compile(CompileState state) => state.Translator.Compile(this, state);

    public QueryCondition In(Parameter parameter) => new(this, Operator.In, parameter);

    public QueryCondition In(Result alias) => new(this, Operator.In, alias);

    public QueryCondition NotIn(Parameter parameter)
    {
        BooleanResult result = new(this, t => t.FnNot);
        return new QueryCondition(result, Operator.In, parameter);
    }

    public QueryCondition NotIn(Result alias)
    {
        BooleanResult result = new(this, t => t.FnNot);
        return new QueryCondition(result, Operator.In, alias);
    }

    public AsResult As(string aliasName) => new(this, aliasName);

    public override string GetFieldName() => throw new NotSupportedException();
    public override Type? GetResultType()
    {
        if (OverridenReturnType is null && Field is not null)
            return Field.GetResultType();

        if (OverridenReturnType is null && Property is not null)
            return Property.SystemReturnType;

        return OverridenReturnType;
    }
}
public class FieldResult<TResult, TResultList, TType> : FieldResult
    where TResult : IPlainPrimitiveResult
    where TResultList : IPrimitiveListResult
{
    internal FieldResult(FieldResult field) : base(field) { }
    public FieldResult(Func<QueryTranslator, string?>? function, object[]? arguments, Type? type) : base(function, arguments, type) { }
    public FieldResult(AliasResult alias, string? fieldName, IEntity? entity, Property? property, Type? overridenReturnType = null) : base(alias, fieldName, entity, property, overridenReturnType) { }
    public FieldResult(AliasResult alias, Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? type = null) : base(alias, function, arguments, type) { }
    public FieldResult(FieldResult field, Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? type = null) : base(field, function, arguments, type) { }

    public TResult Coalesce(TType value) => NewResult(t => t.FnCoalesce, [Parameter.Constant(value)]);
    public TResult Coalesce(TResult value) => NewResult(t => t.FnCoalesce, [value]);
    public TResult Coalesce(Parameter value) => NewResult(t => t.FnCoalesce, [value]);

    public TResultList Collect() => NewList(t => t.FnCollect);
    public TResultList CollectDistinct() => NewList(t => t.FnCollectDistinct);

    private TResult NewResult(Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? overridenReturnType = null) => ResultHelper.Of<TResult>().NewFieldResult((IPrimitiveResult)this, function, arguments, overridenReturnType);
    private TResultList NewList(Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? overridenReturnType = null) => ResultHelper.Of<TResultList>().NewFieldResult((IPrimitiveResult)this, function, arguments, overridenReturnType);
}
