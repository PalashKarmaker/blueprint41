using System;
using System.Collections.Generic;

using Blueprint41;
using Blueprint41.Core;
using Blueprint41.Neo4j.Model;
using Blueprint41.Query;

using m = Domain.Data.Manipulation;

namespace Domain.Data.Query
{
	public partial class Node
	{
		public static SalesTaxRateNode SalesTaxRate { get { return new SalesTaxRateNode(); } }
	}

	public partial class SalesTaxRateNode : Blueprint41.Query.Node
	{
        public static implicit operator QueryCondition(SalesTaxRateNode a)
        {
            return new QueryCondition(a);
        }
        public static QueryCondition operator !(SalesTaxRateNode a)
        {
            return new QueryCondition(a, true);
        } 

		protected override string GetNeo4jLabel()
		{
			return "SalesTaxRate";
		}

		protected override Entity GetEntity()
        {
			return m.SalesTaxRate.Entity;
        }
		public FunctionalId FunctionalId
        {
            get
            {
                return m.SalesTaxRate.Entity.FunctionalId;
            }
        }

		internal SalesTaxRateNode() { }
		internal SalesTaxRateNode(SalesTaxRateAlias alias, bool isReference = false)
		{
			NodeAlias = alias;
			IsReference = isReference;
		}
		internal SalesTaxRateNode(RELATIONSHIP relationship, DirectionEnum direction, string neo4jLabel = null, Entity entity = null) : base(relationship, direction, neo4jLabel, entity) { }
		internal SalesTaxRateNode(RELATIONSHIP relationship, DirectionEnum direction, AliasResult nodeAlias, string neo4jLabel = null, Entity entity = null) : base(relationship, direction, neo4jLabel, entity)
		{
			NodeAlias = nodeAlias;
		}

        public SalesTaxRateNode Where(JsNotation<System.DateTime> ModifiedDate = default, JsNotation<string> Name = default, JsNotation<string> rowguid = default, JsNotation<string> TaxRate = default, JsNotation<string> TaxType = default, JsNotation<string> Uid = default)
        {
            if (InlineConditions is not null || InlineAssignments is not null)
                throw new NotSupportedException("You cannot, at the same time, have inline-assignments and inline-conditions defined on a node.");

            Lazy<SalesTaxRateAlias> alias = new Lazy<SalesTaxRateAlias>(delegate()
            {
                this.Alias(out var a);
                return a;
            });
            List<QueryCondition> conditions = new List<QueryCondition>();
            if (ModifiedDate.HasValue) conditions.Add(new QueryCondition(alias.Value.ModifiedDate, Operator.Equals, ((IValue)ModifiedDate).GetValue()));
            if (Name.HasValue) conditions.Add(new QueryCondition(alias.Value.Name, Operator.Equals, ((IValue)Name).GetValue()));
            if (rowguid.HasValue) conditions.Add(new QueryCondition(alias.Value.rowguid, Operator.Equals, ((IValue)rowguid).GetValue()));
            if (TaxRate.HasValue) conditions.Add(new QueryCondition(alias.Value.TaxRate, Operator.Equals, ((IValue)TaxRate).GetValue()));
            if (TaxType.HasValue) conditions.Add(new QueryCondition(alias.Value.TaxType, Operator.Equals, ((IValue)TaxType).GetValue()));
            if (Uid.HasValue) conditions.Add(new QueryCondition(alias.Value.Uid, Operator.Equals, ((IValue)Uid).GetValue()));

            InlineConditions = conditions.ToArray();

            return this;
        }
        public SalesTaxRateNode Assign(JsNotation<System.DateTime> ModifiedDate = default, JsNotation<string> Name = default, JsNotation<string> rowguid = default, JsNotation<string> TaxRate = default, JsNotation<string> TaxType = default, JsNotation<string> Uid = default)
        {
            if (InlineConditions is not null || InlineAssignments is not null)
                throw new NotSupportedException("You cannot, at the same time, have inline-assignments and inline-conditions defined on a node.");

            Lazy<SalesTaxRateAlias> alias = new Lazy<SalesTaxRateAlias>(delegate()
            {
                this.Alias(out var a);
                return a;
            });
            List<Assignment> assignments = new List<Assignment>();
            if (ModifiedDate.HasValue) assignments.Add(new Assignment(alias.Value.ModifiedDate, ModifiedDate));
            if (Name.HasValue) assignments.Add(new Assignment(alias.Value.Name, Name));
            if (rowguid.HasValue) assignments.Add(new Assignment(alias.Value.rowguid, rowguid));
            if (TaxRate.HasValue) assignments.Add(new Assignment(alias.Value.TaxRate, TaxRate));
            if (TaxType.HasValue) assignments.Add(new Assignment(alias.Value.TaxType, TaxType));
            if (Uid.HasValue) assignments.Add(new Assignment(alias.Value.Uid, Uid));

            InlineAssignments = assignments.ToArray();

            return this;
        }

		public SalesTaxRateNode Alias(out SalesTaxRateAlias alias)
        {
            if (NodeAlias is SalesTaxRateAlias a)
            {
                alias = a;
            }
            else
            {
                alias = new SalesTaxRateAlias(this);
                NodeAlias = alias;
            }
            return this;
        }
		public SalesTaxRateNode Alias(out SalesTaxRateAlias alias, string name)
        {
            if (NodeAlias is SalesTaxRateAlias a)
            {
                a.SetAlias(name);
                alias = a;
            }
            else
            {
                alias = new SalesTaxRateAlias(this, name);
                NodeAlias = alias;
            }
            return this;
        }

		public SalesTaxRateNode UseExistingAlias(AliasResult alias)
		{
			NodeAlias = alias;
            IsReference = true;
			return this;
		}

		public SalesTaxRateIn  In  { get { return new SalesTaxRateIn(this); } }
		public class SalesTaxRateIn
		{
			private SalesTaxRateNode Parent;
			internal SalesTaxRateIn(SalesTaxRateNode parent)
			{
				Parent = parent;
			}
			public IFromIn_SALESTAXRATE_HAS_STATEPROVINCE_REL SALESTAXRATE_HAS_STATEPROVINCE { get { return new SALESTAXRATE_HAS_STATEPROVINCE_REL(Parent, DirectionEnum.In); } }

		}
	}

	public class SalesTaxRateAlias : AliasResult<SalesTaxRateAlias, SalesTaxRateListAlias>
	{
		internal SalesTaxRateAlias(SalesTaxRateNode parent)
		{
			Node = parent;
		}
		internal SalesTaxRateAlias(SalesTaxRateNode parent, string name)
		{
			Node = parent;
			AliasName = name;
		}
		internal void SetAlias(string name) => AliasName = name;

		private  SalesTaxRateAlias(Func<QueryTranslator, string> function, object[] arguments, Type type) : base(function, arguments, type) { }
		private  SalesTaxRateAlias(FieldResult parent, Func<QueryTranslator, string> function, object[] arguments = null, Type type = null) : base(parent, function, arguments, type) { }
		private  SalesTaxRateAlias(AliasResult alias, Func<QueryTranslator, string> function, object[] arguments = null, Type type = null) : base(alias, function, arguments, type)
		{
			Node = alias.Node;
		}

		public Assignment[] Assign(JsNotation<System.DateTime> ModifiedDate = default, JsNotation<string> Name = default, JsNotation<string> rowguid = default, JsNotation<string> TaxRate = default, JsNotation<string> TaxType = default, JsNotation<string> Uid = default)
        {
            List<Assignment> assignments = new List<Assignment>();
			if (ModifiedDate.HasValue) assignments.Add(new Assignment(this.ModifiedDate, ModifiedDate));
			if (Name.HasValue) assignments.Add(new Assignment(this.Name, Name));
			if (rowguid.HasValue) assignments.Add(new Assignment(this.rowguid, rowguid));
			if (TaxRate.HasValue) assignments.Add(new Assignment(this.TaxRate, TaxRate));
			if (TaxType.HasValue) assignments.Add(new Assignment(this.TaxType, TaxType));
			if (Uid.HasValue) assignments.Add(new Assignment(this.Uid, Uid));
            
            return assignments.ToArray();
        }


		public override IReadOnlyDictionary<string, FieldResult> AliasFields
		{
			get
			{
				if (m_AliasFields is null)
				{
					m_AliasFields = new Dictionary<string, FieldResult>()
					{
						{ "TaxType", new StringResult(this, "TaxType", Datastore.AdventureWorks.Model.Entities["SalesTaxRate"], Datastore.AdventureWorks.Model.Entities["SalesTaxRate"].Properties["TaxType"]) },
						{ "TaxRate", new StringResult(this, "TaxRate", Datastore.AdventureWorks.Model.Entities["SalesTaxRate"], Datastore.AdventureWorks.Model.Entities["SalesTaxRate"].Properties["TaxRate"]) },
						{ "Name", new StringResult(this, "Name", Datastore.AdventureWorks.Model.Entities["SalesTaxRate"], Datastore.AdventureWorks.Model.Entities["SalesTaxRate"].Properties["Name"]) },
						{ "rowguid", new StringResult(this, "rowguid", Datastore.AdventureWorks.Model.Entities["SalesTaxRate"], Datastore.AdventureWorks.Model.Entities["SalesTaxRate"].Properties["rowguid"]) },
						{ "ModifiedDate", new DateTimeResult(this, "ModifiedDate", Datastore.AdventureWorks.Model.Entities["SalesTaxRate"], Datastore.AdventureWorks.Model.Entities["SchemaBase"].Properties["ModifiedDate"]) },
						{ "Uid", new StringResult(this, "Uid", Datastore.AdventureWorks.Model.Entities["SalesTaxRate"], Datastore.AdventureWorks.Model.Entities["Neo4jBase"].Properties["Uid"]) },
					};
				}
				return m_AliasFields;
			}
		}
		private IReadOnlyDictionary<string, FieldResult> m_AliasFields = null;

		public SalesTaxRateNode.SalesTaxRateIn In { get { return new SalesTaxRateNode.SalesTaxRateIn(new SalesTaxRateNode(this, true)); } }

		public StringResult TaxType
		{
			get
			{
				if (m_TaxType is null)
					m_TaxType = (StringResult)AliasFields["TaxType"];

				return m_TaxType;
			}
		}
		private StringResult m_TaxType = null;
		public StringResult TaxRate
		{
			get
			{
				if (m_TaxRate is null)
					m_TaxRate = (StringResult)AliasFields["TaxRate"];

				return m_TaxRate;
			}
		}
		private StringResult m_TaxRate = null;
		public StringResult Name
		{
			get
			{
				if (m_Name is null)
					m_Name = (StringResult)AliasFields["Name"];

				return m_Name;
			}
		}
		private StringResult m_Name = null;
		public StringResult rowguid
		{
			get
			{
				if (m_rowguid is null)
					m_rowguid = (StringResult)AliasFields["rowguid"];

				return m_rowguid;
			}
		}
		private StringResult m_rowguid = null;
		public DateTimeResult ModifiedDate
		{
			get
			{
				if (m_ModifiedDate is null)
					m_ModifiedDate = (DateTimeResult)AliasFields["ModifiedDate"];

				return m_ModifiedDate;
			}
		}
		private DateTimeResult m_ModifiedDate = null;
		public StringResult Uid
		{
			get
			{
				if (m_Uid is null)
					m_Uid = (StringResult)AliasFields["Uid"];

				return m_Uid;
			}
		}
		private StringResult m_Uid = null;
		public AsResult As(string aliasName, out SalesTaxRateAlias alias)
		{
			alias = new SalesTaxRateAlias((SalesTaxRateNode)Node)
			{
				AliasName = aliasName
			};
			return this.As(aliasName);
		}
	}

	public class SalesTaxRateListAlias : ListResult<SalesTaxRateListAlias, SalesTaxRateAlias>, IAliasListResult
	{
		private SalesTaxRateListAlias(Func<QueryTranslator, string> function, object[] arguments, Type type) : base(function, arguments, type) { }
		private SalesTaxRateListAlias(FieldResult parent, Func<QueryTranslator, string> function, object[] arguments = null, Type type = null) : base(parent, function, arguments, type) { }
		private SalesTaxRateListAlias(AliasResult alias, Func<QueryTranslator, string> function, object[] arguments = null, Type type = null) : base(alias, function, arguments, type) { }
	}
	public class SalesTaxRateJaggedListAlias : ListResult<SalesTaxRateJaggedListAlias, SalesTaxRateListAlias>, IAliasJaggedListResult
	{
		private SalesTaxRateJaggedListAlias(Func<QueryTranslator, string> function, object[] arguments, Type type) : base(function, arguments, type) { }
		private SalesTaxRateJaggedListAlias(FieldResult parent, Func<QueryTranslator, string> function, object[] arguments = null, Type type = null) : base(parent, function, arguments, type) { }
		private SalesTaxRateJaggedListAlias(AliasResult alias, Func<QueryTranslator, string> function, object[] arguments = null, Type type = null) : base(alias, function, arguments, type) { }
	}
}
