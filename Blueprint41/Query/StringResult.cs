using Blueprint41.Neo4j.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint41.Query;

public partial class StringResult
{
    #region Operators

    public static QueryCondition operator ==(StringResult left, string right)
    {
        return new QueryCondition(left, Operator.Equals, Parameter.Constant(right));
    }
    public static QueryCondition operator ==(StringResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.Equals, right);
    }
    public static QueryCondition operator !=(StringResult left, string right)
    {
        return new QueryCondition(left, Operator.NotEquals, Parameter.Constant(right));
    }
    public static QueryCondition operator !=(StringResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.NotEquals, right);
    }

    public static QueryCondition operator >(StringResult left, string right)
    {
        return new QueryCondition(left, Operator.Greater, Parameter.Constant(right));
    }
    public static QueryCondition operator >(StringResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.Greater, right);
    }
    public static QueryCondition operator <(StringResult left, string right)
    {
        return new QueryCondition(left, Operator.Less, Parameter.Constant(right));
    }
    public static QueryCondition operator <(StringResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.Less, right);
    }

    public static QueryCondition operator >=(StringResult left, string right)
    {
        return new QueryCondition(left, Operator.GreaterOrEqual, Parameter.Constant(right));
    }
    public static QueryCondition operator >=(StringResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.GreaterOrEqual, right);
    }
    public static QueryCondition operator <=(StringResult left, string right)
    {
        return new QueryCondition(left, Operator.LessOrEqual, Parameter.Constant(right));
    }
    public static QueryCondition operator <=(StringResult left, Parameter right)
    {
        return new QueryCondition(left, Operator.LessOrEqual, right);
    }

    public override bool Equals(object? obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();

    #endregion

    public QueryCondition EqualsIgnoreCase(string value) => EqualsIgnoreCase(Parameter.Constant(value));
    public QueryCondition EqualsIgnoreCase(Parameter value) => new(IgnoreCase(this), Operator.Equals, IgnoreCase(value));
    public QueryCondition EqualsIgnoreCase(StringResult value) => new(IgnoreCase(this), Operator.Equals, IgnoreCase(value));
    private static StringResult IgnoreCase(Parameter parameter) => new(t => t.FnIgnoreCase, [parameter], null);
    private static StringResult IgnoreCase(StringResult field) => new(field, t => t.FnIgnoreCase, [field]);

    public StringResult ToUpperCase() => new(this, t => t.FnToUpper);
    public StringResult ToLowerCase() => new(this, t => t.FnToLower);
    public StringResult Reverse() => new(this, t => t.FnReverse);
    public StringResult Trim() => new(this, t => t.FnTrim);
    public StringResult LTrim() => new(this, t => t.FnLeftTrim);
    public StringResult RTrim() => new(this, t => t.FnRightTrim);
    public NumericResult Length() => new(this, t => t.FnSize, null, typeof(long));

    public StringListResult Split(string delimiter) => new(this, t => t.FnSplit, [Parameter.Constant(delimiter)]);
    public StringListResult Split(StringResult delimiter) => new(this, t => t.FnSplit, [delimiter]);
    public StringListResult Split(Parameter delimiter) => new(this, t => t.FnSplit, [delimiter]);

    public StringResult Left(int subLength) => new(this, t => t.FnLeft, [Parameter.Constant(subLength)]);
    public StringResult Left(NumericResult subLength) => new(this, t => t.FnLeft, [subLength]);
    public StringResult Left(Parameter subLength) => new(this, t => t.FnLeft, [subLength]);

    public StringResult Right(int subLength) => new(this, t => t.FnRight, [Parameter.Constant(subLength)]);
    public StringResult Right(NumericResult subLength) => new(this, t => t.FnRight, [subLength]);
    public StringResult Right(Parameter subLength) => new(this, t => t.FnRight, [subLength]);

    public StringResult Substring(int begin) => new(this, t => t.FnSubStringWOutLen, [Parameter.Constant(begin)]);
    public StringResult Substring(NumericResult begin) => new(this, t => t.FnSubStringWOutLen, [begin]);
    public StringResult Substring(Parameter begin) => new(this, t => t.FnSubStringWOutLen, [begin]);

    public QueryCondition In(IEnumerable<string> enumerable) => new(this, Operator.In, Parameter.Constant(enumerable.ToArray(), typeof(string)));

    public QueryCondition NotIn(IEnumerable<string> enumerable) => new(new BooleanResult(this, t => t.FnNot), Operator.In, Parameter.Constant(enumerable.ToArray(), typeof(string)));

    public QueryCondition In(params string[] items) => new(this, Operator.In, Parameter.Constant(items, typeof(string)));

    public StringResult Substring(int begin, int subLength) => new(this, t => t.FnSubString, [Parameter.Constant(begin), Parameter.Constant(subLength)]);
    public StringResult Substring(NumericResult begin, int subLength) => new(this, t => t.FnSubString, [begin, Parameter.Constant(subLength)]);
    public StringResult Substring(Parameter begin, int subLength) => new(this, t => t.FnSubString, [begin, Parameter.Constant(subLength)]);
    public StringResult Substring(int begin, NumericResult subLength) => new(this, t => t.FnSubString, [Parameter.Constant(begin), subLength]);
    public StringResult Substring(NumericResult begin, NumericResult subLength) => new(this, t => t.FnSubString, [begin, subLength]);
    public StringResult Substring(Parameter begin, NumericResult subLength) => new(this, t => t.FnSubString, [begin, subLength]);
    public StringResult Substring(int begin, Parameter subLength) => new(this, t => t.FnSubString, [Parameter.Constant(begin), subLength]);
    public StringResult Substring(NumericResult begin, Parameter subLength) => new(this, t => t.FnSubString, [begin, subLength]);
    public StringResult Substring(Parameter begin, Parameter subLength) => new(this, t => t.FnSubString, [begin, subLength]);

    public StringResult Replace(string search, string replacement) => new(this, t => t.FnReplace, [Parameter.Constant(search), Parameter.Constant(replacement)]);
    public StringResult Replace(StringResult search, StringResult replacement) => new(this, t => t.FnReplace, [search, replacement]);
    public StringResult Replace(Parameter search, Parameter replacement) => new(this, t => t.FnReplace, [search, replacement]);
    public StringResult Replace(string search, StringResult replacement) => new(this, t => t.FnReplace, [Parameter.Constant(search), replacement]);
    public StringResult Replace(string search, Parameter replacement) => new(this, t => t.FnReplace, [Parameter.Constant(search), replacement]);
    public StringResult Replace(StringResult search, string replacement) => new(this, t => t.FnReplace, [search, Parameter.Constant(replacement)]);
    public StringResult Replace(StringResult search, Parameter replacement) => new(this, t => t.FnReplace, [search, replacement]);
    public StringResult Replace(Parameter search, string replacement) => new(this, t => t.FnReplace, [search, Parameter.Constant(replacement)]);
    public StringResult Replace(Parameter search, StringResult replacement) => new(this, t => t.FnReplace, [search, replacement]);

    public QueryCondition StartsWith(string text) => new(this, Operator.StartsWith, text);
    public QueryCondition StartsWith(Parameter param) => new(this, Operator.StartsWith, param);
    public QueryCondition NotStartsWith(string text) => new(new BooleanResult(this, t => t.FnNot), Operator.StartsWith, text);
    public QueryCondition NotStartsWith(Parameter param) => new(new BooleanResult(this, t => t.FnNot), Operator.StartsWith, param);
    public QueryCondition EndsWith(string text) => new(this, Operator.EndsWith, text);
    public QueryCondition EndsWith(Parameter param) => new(this, Operator.EndsWith, param);
    public QueryCondition Contains(string text) => new(this, Operator.Contains, text);
    public QueryCondition Contains(Parameter param) => new(this, Operator.Contains, param);
    public QueryCondition Contains(StringResult text) => new(this, Operator.Contains, text);
    public QueryCondition NotContains(string text) => new(new BooleanResult(this, t => t.FnNot), Operator.Contains, text);
    public QueryCondition NotContains(Parameter param) => new(new BooleanResult(this, t => t.FnNot), Operator.Contains, param);
    public QueryCondition MatchRegex(string text) => new(this, Operator.Match, text);

    public QueryCondition MatchRegex(Parameter param) => new(this, Operator.Match, param);

    public StringResult Concat(params object[] args)
    {
        if (args is null)
            throw new NullReferenceException("value cannot be null");

        int count = args.Length;
        object[] parameters = new object[count];
        for (int index = 0; index < count; index++)
        {
            object arg = args[index];
            if (arg is string)
                parameters[index] = Parameter.Constant<string>((string)arg);
            else
                parameters[index] = arg;
        }

        return new StringResult(this, t => t.FnConcatMultiple(count), parameters, typeof(string));
    }

    public StringResult Min() => new(this, t => t.FnMin);
    public StringResult Max() => new(this, t => t.FnMax);
}
