using Blueprint41.Neo4j.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint41.Query;

public partial class FloatResult
{
    #region Operators

    #region Equals

    public static QueryCondition operator ==(FloatResult left, double right)
    {
        return new QueryCondition(left, Operator.Equals, Parameter.Constant(right));
    }
    public static QueryCondition operator ==(FloatResult left, double? right)
    {
        return new QueryCondition(left, Operator.Equals, Parameter.Constant(right));
    }
    public static QueryCondition operator ==(FloatResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.Equals, right);
    }

    #endregion

    #region Not equals

    public static QueryCondition operator !=(FloatResult left, double right)
    {
        return new QueryCondition(left, Operator.NotEquals, Parameter.Constant(right));
    }
    public static QueryCondition operator !=(FloatResult left, double? right)
    {
        return new QueryCondition(left, Operator.NotEquals, Parameter.Constant(right));
    }
    public static QueryCondition operator !=(FloatResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.NotEquals, right);
    }

    #endregion

    #region Less than

    public static QueryCondition operator <(FloatResult left, double right)
    {
        return new QueryCondition(left, Operator.Less, Parameter.Constant(right));
    }
    public static QueryCondition operator <(FloatResult left, double? right)
    {
        return new QueryCondition(left, Operator.Less, Parameter.Constant(right));
    }
    public static QueryCondition operator <(FloatResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.Less, right);
    }

    #endregion

    #region Less than or equals

    public static QueryCondition operator <=(FloatResult left, double right)
    {
        return new QueryCondition(left, Operator.LessOrEqual, Parameter.Constant(right));
    }
    public static QueryCondition operator <=(FloatResult left, double? right)
    {
        return new QueryCondition(left, Operator.LessOrEqual, Parameter.Constant(right));
    }
    public static QueryCondition operator <=(FloatResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.LessOrEqual, right);
    }

    #endregion

    #region Greater than

    public static QueryCondition operator >(FloatResult left, double right)
    {
        return new QueryCondition(left, Operator.Greater, Parameter.Constant(right));
    }
    public static QueryCondition operator >(FloatResult left, double? right)
    {
        return new QueryCondition(left, Operator.Greater, Parameter.Constant(right));
    }
    public static QueryCondition operator >(FloatResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.Greater, right);
    }

    #endregion

    #region Greater than or equals

    public static QueryCondition operator >=(FloatResult left, double right)
    {
        return new QueryCondition(left, Operator.GreaterOrEqual, Parameter.Constant(right));
    }
    public static QueryCondition operator >=(FloatResult left, double? right)
    {
        return new QueryCondition(left, Operator.GreaterOrEqual, Parameter.Constant(right));
    }
    public static QueryCondition operator >=(FloatResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.GreaterOrEqual, right);
    }

    #endregion

    #region Arithmetic

    public static FloatResult operator +(FloatResult left, long right)
    {
        return new FloatResult(left, t => t.FnAdd, new[] { Parameter.Constant(right) });
    }
    public static FloatResult operator -(FloatResult left, long right)
    {
        return new FloatResult(left, t => t.FnSubtract, new[] { Parameter.Constant(right) });
    }
    public static FloatResult operator *(FloatResult left, long right)
    {
        return new FloatResult(left, t => t.FnMultiply, new[] { Parameter.Constant(right) });
    }
    public static FloatResult operator /(FloatResult left, long right)
    {
        return new FloatResult(left, t => t.FnDivide, new[] { Parameter.Constant(right) });
    }
    public static FloatResult operator %(FloatResult left, long right)
    {
        return new FloatResult(left, t => t.FnModulo, new[] { Parameter.Constant(right) });
    }
    public static FloatResult operator ^(FloatResult left, long right)
    {
        return new FloatResult(left, t => t.FnPower, new[] { Parameter.Constant(right) });
    }
    public static FloatResult operator +(FloatResult left, FloatResult right)
    {
        return new FloatResult(left, t => t.FnAdd, new[] { right });
    }
    public static FloatResult operator -(FloatResult left, FloatResult right)
    {
        return new FloatResult(left, t => t.FnSubtract, new[] { right });
    }
    public static FloatResult operator *(FloatResult left, FloatResult right)
    {
        return new FloatResult(left, t => t.FnMultiply, new[] { right });
    }
    public static FloatResult operator /(FloatResult left, FloatResult right)
    {
        return new FloatResult(left, t => t.FnDivide, new[] { right });
    }
    public static FloatResult operator %(FloatResult left, FloatResult right)
    {
        return new FloatResult(left, t => t.FnModulo, new[] { right });
    }
    public static FloatResult operator ^(FloatResult left, FloatResult right)
    {
        return new FloatResult(left, t => t.FnPower, new[] { right });
    }

    #endregion
    public override bool Equals(object? obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();

    #endregion

    public QueryCondition In(IEnumerable<double> enumerable) => new(this, Operator.In, Parameter.Constant(enumerable.ToArray(), typeof(double)));

    public NumericResult Sign() => new(this, t => t.FnSign, null, typeof(long));
    public FloatResult Abs() => new(this, t => t.FnAbs);
    public FloatResult Sum() => new(this, t => t.FnSum);
    public FloatResult Min() => new(this, t => t.FnMin);
    public FloatResult Max() => new(this, t => t.FnMax);
    public FloatResult Avg() => new(this, t => t.FnAvg, null, typeof(Double));
    public FloatResult PercentileDisc(decimal percentile)
    {
        if (percentile < 0 || percentile > 1)
            throw new ArgumentOutOfRangeException("percentile", percentile, $"The value must be between 0 and 1");

        return new FloatResult(this, t => t.FnPercentileDisc, [Parameter.Constant((float)percentile)]);
    }
    public FloatResult PercentileCont(decimal percentile)
    {
        if (percentile < 0 || percentile > 1)
            throw new ArgumentOutOfRangeException("percentile", percentile, $"The value must be between 0 and 1");

        return new FloatResult(this, t => t.FnPercentileCont, [Parameter.Constant((float)percentile)], typeof(Double));
    }
    public FloatResult StDev() => new(this, t => t.FnStDev, null, typeof(Double));
    public FloatResult StDevP() => new(this, t => t.FnStDevP, null, typeof(Double));
    public NumericResult Round() => new(this, t => t.FnRound, null, typeof(long));
    public FloatResult Sqrt() => new(this, t => t.FnSqrt);
    public FloatResult Sin() => new(this, t => t.FnSin);
    public FloatResult Cos() => new(this, t => t.FnCos);
    public FloatResult Tan() => new(this, t => t.FnTan);
    public FloatResult Cot() => new(this, t => t.FnCot);
    public FloatResult Asin() => new(this, t => t.FnASin);
    public FloatResult Acos() => new(this, t => t.FnACos);
    public FloatResult Atan() => new(this, t => t.FnATan);
    public FloatResult Atan2() => new(this, t => t.FnATan2);
    public FloatResult Haversin() => new(this, t => t.FnHaversin);
    public FloatResult Degrees() => new(this, t => t.FnDegrees);
    public FloatResult Radians() => new(this, t => t.FnRadians);
    public FloatResult Log10() => new(this, t => t.FnLog10);
    public FloatResult Log() => new(this, t => t.FnLog);
    public FloatResult Exp() => new(this, t => t.FnExp);
}
