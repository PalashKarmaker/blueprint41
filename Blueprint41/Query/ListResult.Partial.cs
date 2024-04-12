using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Blueprint41.Query;

	public abstract partial class ListResult<TList, TResult>
	{
		public MiscListResult Extract(Func<TResult, MiscResult> logic)
		{
			TResult itemField = NewResult(t => "item", [], typeof(object));
			MiscResult result = logic.Invoke(itemField);
		
			return new MiscListResult(this, t => t.FnListExtract, [result], typeof(object));
		}
		public StringListResult Extract(Func<TResult, StringResult> logic)
		{
			TResult itemField = NewResult(t => "item", [], typeof(string));
			StringResult result = logic.Invoke(itemField);
		
			return new StringListResult(this, t => t.FnListExtract, [result], typeof(string));
		}
		public BooleanListResult Extract(Func<TResult, BooleanResult> logic)
		{
			TResult itemField = NewResult(t => "item", [], typeof(bool));
			BooleanResult result = logic.Invoke(itemField);
		
			return new BooleanListResult(this, t => t.FnListExtract, [result], typeof(bool));
		}
		public DateTimeListResult Extract(Func<TResult, DateTimeResult> logic)
		{
			TResult itemField = NewResult(t => "item", [], typeof(DateTime));
			DateTimeResult result = logic.Invoke(itemField);
		
			return new DateTimeListResult(this, t => t.FnListExtract, [result], typeof(DateTime));
		}
		public FloatListResult Extract(Func<TResult, FloatResult> logic)
		{
			TResult itemField = NewResult(t => "item", [], typeof(double));
			FloatResult result = logic.Invoke(itemField);
		
			return new FloatListResult(this, t => t.FnListExtract, [result], typeof(double));
		}
		public NumericListResult Extract(Func<TResult, NumericResult> logic)
		{
			TResult itemField = NewResult(t => "item", [], typeof(long));
			NumericResult result = logic.Invoke(itemField);
		
			return new NumericListResult(this, t => t.FnListExtract, [result], typeof(long));
		}
	}
	public abstract partial class ListResult<TList, TResult, TType>
	{
		public MiscListResult Extract(Func<TResult, MiscResult> logic)
		{
			TResult itemField = NewResult(t => "item", [], typeof(TType));
			MiscResult result = logic.Invoke(itemField);

			return new MiscListResult(this, t => t.FnListExtract, [result]);
		}
		public StringListResult Extract(Func<TResult, StringResult> logic)
		{
			TResult itemField = NewResult(t => "item", [], typeof(TType));
			StringResult result = logic.Invoke(itemField);

			return new StringListResult(this, t => t.FnListExtract, [result]);
		}
		public BooleanListResult Extract(Func<TResult, BooleanResult> logic)
		{
			TResult itemField = NewResult(t => "item", [], typeof(TType));
			BooleanResult result = logic.Invoke(itemField);

			return new BooleanListResult(this, t => t.FnListExtract, [result]);
		}
		public DateTimeListResult Extract(Func<TResult, DateTimeResult> logic)
		{
			TResult itemField = NewResult(t => "item", [], typeof(TType));
			DateTimeResult result = logic.Invoke(itemField);

			return new DateTimeListResult(this, t => t.FnListExtract, [result]);
		}
		public FloatListResult Extract(Func<TResult, FloatResult> logic)
		{
			TResult itemField = NewResult(t => "item", [], typeof(TType));
			FloatResult result = logic.Invoke(itemField);

			return new FloatListResult(this, t => t.FnListExtract, [result]);
		}
		public NumericListResult Extract(Func<TResult, NumericResult> logic)
		{
			TResult itemField = NewResult(t => "item", [], typeof(TType));
			NumericResult result = logic.Invoke(itemField);

			return new NumericListResult(this, t => t.FnListExtract, [result]);
		}

    public override Type? GetResultType() => typeof(List<TType>);
}