using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Blueprint41.Core;
using Blueprint41.Neo4j.Model;

namespace Blueprint41.Query;

public partial class AliasResult : Result, IPlainAliasResult
{
    private object[] emptyArguments = [];
    protected internal AliasResult()
    {
        FunctionText = delegate (QueryTranslator t) { return null; };
    }

    protected internal AliasResult(Func<QueryTranslator, string?>? function, object[]? arguments, Type? type) : this((AliasResult)null!, function, arguments, type) { }
    protected internal AliasResult(FieldResult? parent, Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? type = null) : this(parent?.Alias!, function, arguments, type ?? parent?.GetResultType()) { }
    //protected internal AliasResult(FieldResult? parent, Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? type = null) : this(parent?.Alias!, function, arguments, type) { }
    protected internal AliasResult(AliasResult alias, Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? type = null)
    {
        Alias = alias;
        AliasName = alias?.AliasName;
        Node = alias?.Node;
        FunctionText = function ?? delegate (QueryTranslator t) { return null; };
        FunctionArgs = arguments ?? emptyArguments;
        OverridenReturnType = type;
    }

    public AliasResult? Alias { get; private set; }
    internal Func<QueryTranslator, string?> FunctionText { get; private set; }
    internal object[]? FunctionArgs { get; private set; }
    private Type? OverridenReturnType { get; set; }

    public Entity? Entity { get { return Node?.Entity; } }

    public static QueryCondition operator ==(AliasResult a, AliasResult b)
    {
        return new QueryCondition(a, Operator.Equals, b);
    }
    public static QueryCondition operator ==(AliasResult a, Parameter b)
    {
        return new QueryCondition(a, Operator.Equals, b);
    }
    public static QueryCondition operator !=(AliasResult a, AliasResult b)
    {
        return new QueryCondition(a, Operator.NotEquals, b);
    }
    public static QueryCondition operator !=(AliasResult a, Parameter b)
    {
        return new QueryCondition(a, Operator.NotEquals, b);
    }

    public override bool Equals(object? obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();

    public string? AliasName { get; protected internal set; }
    public Node? Node { get; protected internal set; }

    protected internal override void Compile(CompileState state) => state.Translator.Compile(this, state);

    public QueryCondition HasLabel(string label) => new(this, Operator.HasLabel, new Literal(label));

    public static QueryCondition Not(QueryCondition condition) => new(string.Empty, Operator.Not, condition);

    public AsResult As(string aliasName) => new(this, aliasName);
    public AsResult As(string aliasName, out AliasResult alias)
    {
        alias = new AliasResult(this, null)
        {
            AliasName = aliasName,
        };
        return new AsResult(this, aliasName);
    }

    public StringResult ElementId
    {
        get
        {
            return new StringResult(t => t.FnElementId, [this], null);
        }
    }

    public AsResult Properties(string alias) => new(new MiscResult(t => t.FnProperties, [this], null), alias);
    public AsResult Properties(string alias, out PropertiesAliasResult propertiesAlias)
    {
        propertiesAlias = new PropertiesAliasResult()
        {
            AliasName = alias
        };
        return new AsResult(new MiscResult(t => t.FnProperties, [this], null), alias);
    }

    public override string? GetFieldName() => AliasName;

    public override Type? GetResultType() => OverridenReturnType ?? Alias?.GetResultType() ?? null;

    new public StringResult ToString()
    {
        if (AliasName is null)
            throw new InvalidOperationException("You cannot use the labels function in this context.");

        return new StringResult(t => t.FnParam1, [Parameter.Constant(AliasName)], typeof(string));
    }

    public virtual IReadOnlyDictionary<string, FieldResult> AliasFields { get { return emptyAliasFields; }  }
    private static Dictionary<string, FieldResult> emptyAliasFields = [];

    public StringListResult Labels()
    {
        if (AliasName is null)
            throw new InvalidOperationException("You cannot use the labels function in this context.");

        return new StringListResult(t => t.FnLabels, [Parameter.Constant(AliasName)], typeof(string));
    }

    AsResult IResult.As<T>(string aliasName, out T alias)
    {
        if (typeof(T) != this.GetType())
            throw new NotSupportedException($"The type of the result ({this.GetType().Name}) must match the generic type of As<{typeof(T).Name}>");

        AsResult retval = ResultHelper.Of<T>().As((T)(object)this, aliasName, out T tmp);
        alias = tmp;
        return retval;
    }
}
public abstract partial class AliasResult<T, TList> : AliasResult
    where T : AliasResult<T, TList>
    where TList : ListResult<TList, T>, IAliasListResult
{
    protected internal AliasResult()
    {
    }

    protected AliasResult(Func<QueryTranslator, string?>? function, object[]? arguments, Type? type) : base(function, arguments, type) { }
    protected AliasResult(FieldResult? parent, Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? type = null) : base(parent, function, arguments, type) { }
    protected AliasResult(AliasResult parent, Func<QueryTranslator, string?>? function, object[]? arguments = null, Type? type = null) : base(parent, function, arguments, type) { }

    public TList Collect() => ResultHelper.Of<TList>().NewAliasResult(this, t => t.FnCollect);
    public TList CollectDistinct() => ResultHelper.Of<TList>().NewAliasResult(this, t => t.FnCollectDistinct);

    public NumericResult Count() => new(this, t => t.FnCount, null, typeof(long));
    public NumericResult CountDistinct() => new(this, t => t.FnCountDistinct, null, typeof(long));

    public T Coalesce(T other) => ResultHelper.Of<T>().NewAliasResult(this, t => t.FnCoalesce, [other], null);
}
