using System;
using System.Collections.Generic;

using Blueprint41;
using Blueprint41.Query;

namespace Domain.Data.Query
{
public partial class PURCHASEORDERDETAIL_HAS_PRODUCT_REL : RELATIONSHIP, IFromIn_PURCHASEORDERDETAIL_HAS_PRODUCT_REL, IFromOut_PURCHASEORDERDETAIL_HAS_PRODUCT_REL	{
		public override string NEO4J_TYPE
		{
			get
			{
				return "HAS_PRODUCT";
			}
		}
		public override AliasResult RelationshipAlias { get; protected set; }
		
		internal PURCHASEORDERDETAIL_HAS_PRODUCT_REL(Blueprint41.Query.Node parent, DirectionEnum direction) : base(parent, direction) { }

		public PURCHASEORDERDETAIL_HAS_PRODUCT_REL Alias(out PURCHASEORDERDETAIL_HAS_PRODUCT_ALIAS alias)
		{
			alias = new PURCHASEORDERDETAIL_HAS_PRODUCT_ALIAS(this);
			RelationshipAlias = alias;
			return this;
		} 
		public PURCHASEORDERDETAIL_HAS_PRODUCT_REL Repeat(int maxHops)
		{
			return Repeat(1, maxHops);
		}
		public new PURCHASEORDERDETAIL_HAS_PRODUCT_REL Repeat(int minHops, int maxHops)
		{
			base.Repeat(minHops, maxHops);
			return this;
		}

		IFromIn_PURCHASEORDERDETAIL_HAS_PRODUCT_REL IFromIn_PURCHASEORDERDETAIL_HAS_PRODUCT_REL.Alias(out PURCHASEORDERDETAIL_HAS_PRODUCT_ALIAS alias)
		{
			return Alias(out alias);
		}
		IFromOut_PURCHASEORDERDETAIL_HAS_PRODUCT_REL IFromOut_PURCHASEORDERDETAIL_HAS_PRODUCT_REL.Alias(out PURCHASEORDERDETAIL_HAS_PRODUCT_ALIAS alias)
		{
			return Alias(out alias);
		}
		IFromIn_PURCHASEORDERDETAIL_HAS_PRODUCT_REL IFromIn_PURCHASEORDERDETAIL_HAS_PRODUCT_REL.Repeat(int maxHops)
		{
			return Repeat(maxHops);
		}
		IFromIn_PURCHASEORDERDETAIL_HAS_PRODUCT_REL IFromIn_PURCHASEORDERDETAIL_HAS_PRODUCT_REL.Repeat(int minHops, int maxHops)
		{
			return Repeat(minHops, maxHops);
		}
		IFromOut_PURCHASEORDERDETAIL_HAS_PRODUCT_REL IFromOut_PURCHASEORDERDETAIL_HAS_PRODUCT_REL.Repeat(int maxHops)
		{
			return Repeat(maxHops);
		}
		IFromOut_PURCHASEORDERDETAIL_HAS_PRODUCT_REL IFromOut_PURCHASEORDERDETAIL_HAS_PRODUCT_REL.Repeat(int minHops, int maxHops)
		{
			return Repeat(minHops, maxHops);
		}


		public PURCHASEORDERDETAIL_HAS_PRODUCT_IN In { get { return new PURCHASEORDERDETAIL_HAS_PRODUCT_IN(this); } }
		public class PURCHASEORDERDETAIL_HAS_PRODUCT_IN
		{
			private PURCHASEORDERDETAIL_HAS_PRODUCT_REL Parent;
			internal PURCHASEORDERDETAIL_HAS_PRODUCT_IN(PURCHASEORDERDETAIL_HAS_PRODUCT_REL parent)
			{
				Parent = parent;
			}

			public PurchaseOrderDetailNode PurchaseOrderDetail { get { return new PurchaseOrderDetailNode(Parent, DirectionEnum.In); } }
		}

		public PURCHASEORDERDETAIL_HAS_PRODUCT_OUT Out { get { return new PURCHASEORDERDETAIL_HAS_PRODUCT_OUT(this); } }
		public class PURCHASEORDERDETAIL_HAS_PRODUCT_OUT
		{
			private PURCHASEORDERDETAIL_HAS_PRODUCT_REL Parent;
			internal PURCHASEORDERDETAIL_HAS_PRODUCT_OUT(PURCHASEORDERDETAIL_HAS_PRODUCT_REL parent)
			{
				Parent = parent;
			}

			public ProductNode Product { get { return new ProductNode(Parent, DirectionEnum.Out); } }
		}
	}

	public interface IFromIn_PURCHASEORDERDETAIL_HAS_PRODUCT_REL
	{
		IFromIn_PURCHASEORDERDETAIL_HAS_PRODUCT_REL Alias(out PURCHASEORDERDETAIL_HAS_PRODUCT_ALIAS alias);
		IFromIn_PURCHASEORDERDETAIL_HAS_PRODUCT_REL Repeat(int maxHops);
		IFromIn_PURCHASEORDERDETAIL_HAS_PRODUCT_REL Repeat(int minHops, int maxHops);

		PURCHASEORDERDETAIL_HAS_PRODUCT_REL.PURCHASEORDERDETAIL_HAS_PRODUCT_OUT Out { get; }
	}
	public interface IFromOut_PURCHASEORDERDETAIL_HAS_PRODUCT_REL
	{
		IFromOut_PURCHASEORDERDETAIL_HAS_PRODUCT_REL Alias(out PURCHASEORDERDETAIL_HAS_PRODUCT_ALIAS alias);
		IFromOut_PURCHASEORDERDETAIL_HAS_PRODUCT_REL Repeat(int maxHops);
		IFromOut_PURCHASEORDERDETAIL_HAS_PRODUCT_REL Repeat(int minHops, int maxHops);

		PURCHASEORDERDETAIL_HAS_PRODUCT_REL.PURCHASEORDERDETAIL_HAS_PRODUCT_IN In { get; }
	}

	public class PURCHASEORDERDETAIL_HAS_PRODUCT_ALIAS : AliasResult
	{
		private PURCHASEORDERDETAIL_HAS_PRODUCT_REL Parent;

		internal PURCHASEORDERDETAIL_HAS_PRODUCT_ALIAS(PURCHASEORDERDETAIL_HAS_PRODUCT_REL parent)
		{
			Parent = parent;

			CreationDate = new RelationFieldResult(this, "CreationDate");
		}

        public Assignment[] Assign(JsNotation<DateTime> CreationDate = default)
        {
            List<Assignment> assignments = new List<Assignment>();
            if (CreationDate.HasValue) assignments.Add(new Assignment(this.CreationDate, CreationDate));

            return assignments.ToArray();
        }

		public RelationFieldResult CreationDate { get; private set; } 
	}
}
