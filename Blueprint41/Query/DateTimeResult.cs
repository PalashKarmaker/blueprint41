using Blueprint41.Core;
using Blueprint41.Neo4j.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint41.Query;

public partial class DateTimeResult
{
    #region Operators

    #region Equals

    public static QueryCondition operator ==(DateTimeResult left, DateTime right)
    {
        return new QueryCondition(left, Operator.Equals, Parameter.Constant(right));
    }
    public static QueryCondition operator ==(DateTimeResult left, DateTime? right)
    {
        return new QueryCondition(left, Operator.Equals, Parameter.Constant(right));
    }
    public static QueryCondition operator ==(DateTimeResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.Equals, right);
    }
    public static QueryCondition operator ==(DateTimeResult left, DateTimeResult right)
    {
        return new QueryCondition(left, Operator.Equals, right);
    }

    #endregion

    #region Not equals

    public static QueryCondition operator !=(DateTimeResult left, DateTime right)
    {
        return new QueryCondition(left, Operator.NotEquals, Parameter.Constant(right));
    }
    public static QueryCondition operator !=(DateTimeResult left, DateTime? right)
    {
        return new QueryCondition(left, Operator.NotEquals, Parameter.Constant(right));
    }
    public static QueryCondition operator !=(DateTimeResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.NotEquals, right);
    }
    public static QueryCondition operator !=(DateTimeResult left, DateTimeResult right)
    {
        return new QueryCondition(left, Operator.NotEquals, right);
    }

    #endregion

    #region Less than

    public static QueryCondition operator <(DateTimeResult left, DateTime right)
    {
        return new QueryCondition(left, Operator.Less, Parameter.Constant(right));
    }
    public static QueryCondition operator <(DateTimeResult left, DateTime? right)
    {
        return new QueryCondition(left, Operator.Less, Parameter.Constant(right));
    }
    public static QueryCondition operator <(DateTimeResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.Less, right);
    }
    public static QueryCondition operator <(DateTimeResult left, DateTimeResult right)
    {
        return new QueryCondition(left, Operator.Less, right);
    }
    #endregion

    #region Less than or equals

    public static QueryCondition operator <=(DateTimeResult left, DateTime right)
    {
        return new QueryCondition(left, Operator.LessOrEqual, Parameter.Constant(right));
    }
    public static QueryCondition operator <=(DateTimeResult left, DateTime? right)
    {
        return new QueryCondition(left, Operator.LessOrEqual, Parameter.Constant(right));
    }
    public static QueryCondition operator <=(DateTimeResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.LessOrEqual, right);
    }
    public static QueryCondition operator <=(DateTimeResult left, DateTimeResult right)
    {
        return new QueryCondition(left, Operator.LessOrEqual, right);
    }

    #endregion

    #region Greater than

    public static QueryCondition operator >(DateTimeResult left, DateTime right)
    {
        return new QueryCondition(left, Operator.Greater, Parameter.Constant(right));
    }
    public static QueryCondition operator >(DateTimeResult left, DateTime? right)
    {
        return new QueryCondition(left, Operator.Greater, Parameter.Constant(right));
    }
    public static QueryCondition operator >(DateTimeResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.Greater, right);
    }
    public static QueryCondition operator >(DateTimeResult left, DateTimeResult right)
    {
        return new QueryCondition(left, Operator.Greater, right);
    }

    #endregion

    #region Greater than or equals

    public static QueryCondition operator >=(DateTimeResult left, DateTime right)
    {
        return new QueryCondition(left, Operator.GreaterOrEqual, Parameter.Constant(right));
    }
    public static QueryCondition operator >=(DateTimeResult left, DateTime? right)
    {
        return new QueryCondition(left, Operator.GreaterOrEqual, Parameter.Constant(right));
    }
    public static QueryCondition operator >=(DateTimeResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.GreaterOrEqual, right);
    }
    public static QueryCondition operator >=(DateTimeResult left, DateTimeResult right)
    {
        return new QueryCondition(left, Operator.GreaterOrEqual, right);
    }

    #endregion

    public override bool Equals(object? obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();

    #endregion

    public QueryCondition In(IEnumerable<DateTime> enumerable) => new(this, Operator.In, Parameter.Constant(enumerable.ToArray(), typeof(DateTime)));

    public DateTimeResult Coalesce(RelationFieldResult value) => new(this, t => t.FnCoalesce, [value]);
    public DateTimeResult Min() => new(this, t => t.FnMin);
    public DateTimeResult Max() => new(this, t => t.FnMax);
    public DateTimeResult ConvertMinToNull() => new(this, t => t.FnConvertMinOrMaxToNull, [MinDateTime.Value]);
    public DateTimeResult ConvertMaxToNull() => new(this, t => t.FnConvertMinOrMaxToNull, [MaxDateTime.Value]);
    public DateTimeResult ConvertMinOrMaxToNull() => new(this, t => t.FnConvertMinAndMaxToNull, [MinDateTime.Value, MaxDateTime.Value]);

    private Lazy<Parameter> MinDateTime = new(delegate()
    {
        return Parameter.Constant(Conversion<DateTime, long>.Convert(DateTime.MinValue));
    }, true);
    private Lazy<Parameter> MaxDateTime = new(delegate ()
    {
        return Parameter.Constant(Conversion<DateTime, long>.Convert(DateTime.MaxValue));
    }, true);
}
