﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Blueprint41.DatastoreTemplates
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using Blueprint41;
    using Blueprint41.Core;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class Domain_Data_Relationship : GeneratorBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("using System;\r\nusing System.Collections.Generic;\r\n\r\nusing Blueprint41;\r\nusing Blu" +
                    "eprint41.Query;\r\n\r\nnamespace ");
            
            #line 13 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Settings.FullQueryNamespace));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\npublic partial class ");
            
            #line 15 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL : RELATIONSHIP, IFromIn_");
            
            #line 15 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL, IFromOut_");
            
            #line 15 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL");
            
            #line 15 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
 if (DALRelation.InEntity == DALRelation.OutEntity) { 
            
            #line default
            #line hidden
            this.Write(", IFromAny_");
            
            #line 15 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL");
            
            #line 15 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
 }
            
            #line default
            #line hidden
            this.Write("\t{\r\n\t\tpublic override string NEO4J_TYPE\r\n\t\t{\r\n\t\t\tget\r\n\t\t\t{\r\n\t\t\t\treturn \"");
            
            #line 21 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Neo4JRelationshipType));
            
            #line default
            #line hidden
            this.Write("\";\r\n\t\t\t}\r\n\t\t}\r\n\t\tpublic override AliasResult RelationshipAlias { get; protected s" +
                    "et; }\r\n\t\t\r\n\t\tinternal ");
            
            #line 26 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL(Blueprint41.Query.Node parent, DirectionEnum direction) : base(parent, direc" +
                    "tion) { }\r\n\r\n\t\tpublic ");
            
            #line 28 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL Alias(out ");
            
            #line 28 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_ALIAS alias)\r\n\t\t{\r\n\t\t\talias = new ");
            
            #line 30 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_ALIAS(this);\r\n\t\t\tRelationshipAlias = alias;\r\n\t\t\treturn this;\r\n\t\t} \r\n\t\tpublic ");
            
            #line 34 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL Repeat(int maxHops)\r\n\t\t{\r\n\t\t\treturn Repeat(1, maxHops);\r\n\t\t}\r\n\t\tpublic new ");
            
            #line 38 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL Repeat(int minHops, int maxHops)\r\n\t\t{\r\n\t\t\tbase.Repeat(minHops, maxHops);\r\n\t\t" +
                    "\treturn this;\r\n\t\t}\r\n\r\n\t\tIFromIn_");
            
            #line 44 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL IFromIn_");
            
            #line 44 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL.Alias(out ");
            
            #line 44 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_ALIAS alias)\r\n\t\t{\r\n\t\t\treturn Alias(out alias);\r\n\t\t}\r\n\t\tIFromOut_");
            
            #line 48 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL IFromOut_");
            
            #line 48 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL.Alias(out ");
            
            #line 48 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_ALIAS alias)\r\n\t\t{\r\n\t\t\treturn Alias(out alias);\r\n\t\t}\r\n\t\tIFromIn_");
            
            #line 52 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL IFromIn_");
            
            #line 52 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL.Repeat(int maxHops)\r\n\t\t{\r\n\t\t\treturn Repeat(maxHops);\r\n\t\t}\r\n\t\tIFromIn_");
            
            #line 56 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL IFromIn_");
            
            #line 56 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL.Repeat(int minHops, int maxHops)\r\n\t\t{\r\n\t\t\treturn Repeat(minHops, maxHops);\r\n" +
                    "\t\t}\r\n\t\tIFromOut_");
            
            #line 60 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL IFromOut_");
            
            #line 60 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL.Repeat(int maxHops)\r\n\t\t{\r\n\t\t\treturn Repeat(maxHops);\r\n\t\t}\r\n\t\tIFromOut_");
            
            #line 64 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL IFromOut_");
            
            #line 64 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL.Repeat(int minHops, int maxHops)\r\n\t\t{\r\n\t\t\treturn Repeat(minHops, maxHops);\r\n" +
                    "\t\t}\r\n\r\n");
            
            #line 69 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	if (DALRelation.InEntity == DALRelation.OutEntity)
	{

            
            #line default
            #line hidden
            this.Write("\t\tIFromAny_");
            
            #line 73 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL IFromAny_");
            
            #line 73 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL.Alias(out ");
            
            #line 73 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_ALIAS alias)\r\n\t\t{\r\n\t\t\treturn Alias(out alias);\r\n\t\t}\r\n\t\tIFromAny_");
            
            #line 77 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL IFromAny_");
            
            #line 77 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL.Repeat(int maxHops)\r\n\t\t{\r\n\t\t\treturn Repeat(maxHops);\r\n\t\t}\r\n\t\tIFromAny_");
            
            #line 81 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL IFromAny_");
            
            #line 81 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL.Repeat(int minHops, int maxHops)\r\n\t\t{\r\n\t\t\treturn Repeat(minHops, maxHops);\r\n" +
                    "\t\t}\r\n");
            
            #line 85 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	}

            
            #line default
            #line hidden
            this.Write("\r\n\t\tpublic ");
            
            #line 89 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_IN In { get { return new ");
            
            #line 89 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_IN(this); } }\r\n\t\tpublic class ");
            
            #line 90 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_IN\r\n\t\t{\r\n\t\t\tprivate ");
            
            #line 92 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL Parent;\r\n\t\t\tinternal ");
            
            #line 93 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_IN(");
            
            #line 93 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL parent)\r\n\t\t\t{\r\n\t\t\t\tParent = parent;\r\n\t\t\t}\r\n\r\n");
            
            #line 98 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	foreach (Entity entity in DALRelation.InEntity.GetSubclassesOrSelf())
	{

            
            #line default
            #line hidden
            this.Write("\t\t\tpublic ");
            
            #line 102 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.Name));
            
            #line default
            #line hidden
            this.Write("Node ");
            
            #line 102 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.Name));
            
            #line default
            #line hidden
            this.Write(" { get { return new ");
            
            #line 102 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.Name));
            
            #line default
            #line hidden
            this.Write("Node(Parent, DirectionEnum.In); } }\r\n");
            
            #line 103 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	}

            
            #line default
            #line hidden
            this.Write("\t\t}\r\n\r\n\t\tpublic ");
            
            #line 108 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_OUT Out { get { return new ");
            
            #line 108 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_OUT(this); } }\r\n\t\tpublic class ");
            
            #line 109 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_OUT\r\n\t\t{\r\n\t\t\tprivate ");
            
            #line 111 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL Parent;\r\n\t\t\tinternal ");
            
            #line 112 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_OUT(");
            
            #line 112 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL parent)\r\n\t\t\t{\r\n\t\t\t\tParent = parent;\r\n\t\t\t}\r\n\r\n");
            
            #line 117 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	foreach (Entity entity in DALRelation.OutEntity.GetSubclassesOrSelf())
	{
		if(entity.IsVirtual && DALRelation.OutEntity != entity)
		{

            
            #line default
            #line hidden
            this.Write("\t\t\t[Obsolete(\"This relationship is virtual, consider making entity ");
            
            #line 123 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.Name));
            
            #line default
            #line hidden
            this.Write(" concrete or exit this relationship via ");
            
            #line 123 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.OutEntity.Name));
            
            #line default
            #line hidden
            this.Write(".\", true)]\r\n");
            
            #line 124 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

		}

            
            #line default
            #line hidden
            this.Write("\t\t\tpublic ");
            
            #line 127 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.Name));
            
            #line default
            #line hidden
            this.Write("Node ");
            
            #line 127 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.Name));
            
            #line default
            #line hidden
            this.Write(" { get { return new ");
            
            #line 127 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.Name));
            
            #line default
            #line hidden
            this.Write("Node(Parent, DirectionEnum.Out); } }\r\n");
            
            #line 128 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	}

            
            #line default
            #line hidden
            this.Write("\t\t}\r\n");
            
            #line 132 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	if (DALRelation.InEntity == DALRelation.OutEntity)
	{

            
            #line default
            #line hidden
            this.Write("\r\n\t\tpublic ");
            
            #line 137 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_ANY Any { get { return new ");
            
            #line 137 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_ANY(this); } }\r\n\t\tpublic class ");
            
            #line 138 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_ANY\r\n\t\t{\r\n\t\t\tprivate ");
            
            #line 140 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL Parent;\r\n\t\t\tinternal ");
            
            #line 141 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_ANY(");
            
            #line 141 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL parent)\r\n\t\t\t{\r\n\t\t\t\tParent = parent;\r\n\t\t\t}\r\n\r\n");
            
            #line 146 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	foreach (Entity entity in DALRelation.InEntity.GetSubclassesOrSelf())
	{

            
            #line default
            #line hidden
            this.Write("\t\t\tpublic ");
            
            #line 150 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.Name));
            
            #line default
            #line hidden
            this.Write("Node ");
            
            #line 150 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.Name));
            
            #line default
            #line hidden
            this.Write(" { get { return new ");
            
            #line 150 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.Name));
            
            #line default
            #line hidden
            this.Write("Node(Parent, DirectionEnum.None); } }\r\n");
            
            #line 151 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	}

            
            #line default
            #line hidden
            this.Write("\t\t}\r\n");
            
            #line 155 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	}

            
            #line default
            #line hidden
            this.Write("\t}\r\n\r\n\tpublic interface IFromIn_");
            
            #line 160 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL\r\n\t{\r\n\t\tIFromIn_");
            
            #line 162 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL Alias(out ");
            
            #line 162 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_ALIAS alias);\r\n\t\tIFromIn_");
            
            #line 163 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL Repeat(int maxHops);\r\n\t\tIFromIn_");
            
            #line 164 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL Repeat(int minHops, int maxHops);\r\n\r\n\t\t");
            
            #line 166 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL.");
            
            #line 166 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_OUT Out { get; }\r\n\t}\r\n\tpublic interface IFromOut_");
            
            #line 168 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL\r\n\t{\r\n\t\tIFromOut_");
            
            #line 170 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL Alias(out ");
            
            #line 170 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_ALIAS alias);\r\n\t\tIFromOut_");
            
            #line 171 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL Repeat(int maxHops);\r\n\t\tIFromOut_");
            
            #line 172 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL Repeat(int minHops, int maxHops);\r\n\r\n\t\t");
            
            #line 174 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL.");
            
            #line 174 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_IN In { get; }\r\n\t}\r\n");
            
            #line 176 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	if (DALRelation.InEntity == DALRelation.OutEntity)
	{

            
            #line default
            #line hidden
            this.Write("\tpublic interface IFromAny_");
            
            #line 180 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL\r\n\t{\r\n\t\tIFromAny_");
            
            #line 182 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL Alias(out ");
            
            #line 182 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_ALIAS alias);\r\n\t\tIFromAny_");
            
            #line 183 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL Repeat(int maxHops);\r\n\t\tIFromAny_");
            
            #line 184 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL Repeat(int minHops, int maxHops);\r\n\r\n\t\t");
            
            #line 186 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL.");
            
            #line 186 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_ANY Any { get; }\r\n\t}\r\n");
            
            #line 188 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	}

            
            #line default
            #line hidden
            this.Write("\r\n\tpublic class ");
            
            #line 192 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_ALIAS : AliasResult\r\n\t{\r\n\t\tprivate ");
            
            #line 194 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL Parent;\r\n\r\n\t\tinternal ");
            
            #line 196 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_ALIAS(");
            
            #line 196 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALRelation.Name));
            
            #line default
            #line hidden
            this.Write("_REL parent)\r\n\t\t{\r\n\t\t\tParent = parent;\r\n\r\n\t\t\tCreationDate = new RelationFieldResu" +
                    "lt(this, \"CreationDate\");\r\n");
            
            #line 201 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	List<string> fields = new List<string>() { "CreationDate" };

	if (DALRelation.IsTimeDependent)
	{
		fields.Add("StartDate");
		fields.Add("EndDate");

            
            #line default
            #line hidden
            this.Write("\t\t\tStartDate    = new RelationFieldResult(this, \"StartDate\");\r\n\t\t\tEndDate      = " +
                    "new RelationFieldResult(this, \"EndDate\");\r\n");
            
            #line 211 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	}

            
            #line default
            #line hidden
            this.Write("\t\t}\r\n\r\n        public Assignment[] Assign(");
            
            #line 216 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(string.Join(", ", fields.Select(item => $"JsNotation<DateTime> {item} = default"))));
            
            #line default
            #line hidden
            this.Write(")\r\n        {\r\n            List<Assignment> assignments = new List<Assignment>();\r" +
                    "\n            if (CreationDate.HasValue) assignments.Add(new Assignment(this.Crea" +
                    "tionDate, CreationDate));\r\n");
            
            #line 220 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	if (DALRelation.IsTimeDependent)
	{

            
            #line default
            #line hidden
            this.Write("            if (StartDate.HasValue) assignments.Add(new Assignment(this.StartDate" +
                    ", StartDate));\r\n            if (EndDate.HasValue) assignments.Add(new Assignment" +
                    "(this.EndDate, EndDate));\r\n");
            
            #line 226 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	}

            
            #line default
            #line hidden
            this.Write("\r\n            return assignments.ToArray();\r\n        }\r\n\r\n\t\tpublic RelationFieldR" +
                    "esult CreationDate { get; private set; } \r\n");
            
            #line 234 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	if (DALRelation.IsTimeDependent)
	{

            
            #line default
            #line hidden
            this.Write("\t\tpublic RelationFieldResult StartDate { get; private set; } \r\n\t\tpublic RelationF" +
                    "ieldResult EndDate   { get; private set; } \r\n");
            
            #line 240 "C:\_CirclesArrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Relationship.tt"

	}

            
            #line default
            #line hidden
            this.Write("\t}\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
