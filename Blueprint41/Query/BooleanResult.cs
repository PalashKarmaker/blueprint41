﻿using Blueprint41.Neo4j.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint41.Query;

public partial class BooleanResult
{
    #region Operator

    public static QueryCondition operator ==(BooleanResult left, bool right)
    {
        return new QueryCondition(left, Operator.Equals, Parameter.Constant(right));
    }
    public static QueryCondition operator ==(BooleanResult left, bool? right)
    {
        return new QueryCondition(left, Operator.Equals, Parameter.Constant(right));
    }
    public static QueryCondition operator ==(BooleanResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.Equals, right);
    }
    public static QueryCondition operator !=(BooleanResult left, bool right)
    {
        return new QueryCondition(left, Operator.NotEquals, Parameter.Constant(right));
    }
    public static QueryCondition operator !=(BooleanResult left, bool? right)
    {
        return new QueryCondition(left, Operator.NotEquals, Parameter.Constant(right));
    }
    public static QueryCondition operator !=(BooleanResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.NotEquals, right);
    }
    public static BooleanResult operator !(BooleanResult field)
    {
        return new BooleanResult(field, t => t.FnNot, null, typeof(bool));
    }

    public override bool Equals(object? obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();

    #endregion

    public override NumericResult ToInteger() => new(t => t.FnConvertToBoolean, [this], typeof(long));

    private static readonly Parameter ZERO = Parameter.Constant<long>(0);
    private static readonly Parameter ONE = Parameter.Constant<long>(1);
}
