﻿using Blueprint41.Core;
using Blueprint41.Neo4j.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint41.Query;

public class RelationFieldResult : FieldResult, IPlainPrimitiveResult
{
    public static explicit operator DateTimeResult(RelationFieldResult from)
    {
        return new DateTimeResult(from);
    }
    public static explicit operator RelationFieldResult(DateTimeResult from)
    {
        return new RelationFieldResult(from);
    }

    public static QueryCondition operator ==(RelationFieldResult left, DateTime right)
    {
        return new QueryCondition(left, Operator.Equals, Parameter.Constant(right));
    }
    public static QueryCondition operator ==(RelationFieldResult left, DateTime? right)
    {
        return new QueryCondition(left, Operator.Equals, Parameter.Constant(right));
    }
    public static QueryCondition operator ==(RelationFieldResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.Equals, right);
    }
    public static QueryCondition operator !=(RelationFieldResult left, DateTime right)
    {
        return new QueryCondition(left, Operator.NotEquals, Parameter.Constant(right));
    }
    public static QueryCondition operator !=(RelationFieldResult left, DateTime? right)
    {
        return new QueryCondition(left, Operator.NotEquals, Parameter.Constant(right));
    }
    public static QueryCondition operator !=(RelationFieldResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.NotEquals, right);
    }
    public static QueryCondition operator >=(RelationFieldResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.GreaterOrEqual, right);
    }
    public static QueryCondition operator <=(RelationFieldResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.LessOrEqual, right);
    }

    public override bool Equals(object? obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();

    internal RelationFieldResult(FieldResult field) : base(field) { }
    public RelationFieldResult(Func<QueryTranslator, string?>? function, object[] arguments, Type type) : base(function, arguments, type) { }
    public RelationFieldResult(AliasResult alias, string? fieldName) : base(alias, fieldName, null, null, typeof(DateTime)) { }
    public RelationFieldResult(FieldResult field, Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? type = null) : base(field, function, arguments, type) { }

    protected internal override void Compile(CompileState state) => state.Translator.Compile(this, state);

    public QueryCondition In(IEnumerable<DateTime> enumerable) => new(this, Operator.In, Parameter.Constant(enumerable.ToArray(), typeof(DateTime)));

    public DateTimeResult Coalesce(DateTime value) => new(this, t => t.FnCoalesce, [Parameter.Constant(Conversion<DateTime, long>.Convert(value))]);
    public DateTimeResult Coalesce(RelationFieldResult value) => new(this, t => t.FnCoalesce, [value]);
    public DateTimeResult Coalesce(DateTimeResult value) => new(this, t => t.FnCoalesce, [value]);
    public DateTimeResult Coalesce(Parameter value) => new(this, t => t.FnCoalesce, [value]);

    public override Type GetResultType() => typeof(DateTime);

    public AsResult As(string aliasName, out RelationFieldResult alias)
    {
        AliasResult aliasResult = new(this, null)
        {
            AliasName = aliasName
        };

        alias = new RelationFieldResult(aliasResult, null);
        return this.As(aliasName);
    }
    AsResult IResult.As<T>(string aliasName, out T alias) => throw new NotSupportedException();
}
