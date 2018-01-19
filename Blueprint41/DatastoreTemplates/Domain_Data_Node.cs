﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 15.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Blueprint41.DatastoreTemplates
{
    using System.Linq;
    using System.Collections.Generic;
    using Blueprint41;
    using Blueprint41.Core;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public partial class Domain_Data_Node : GeneratorBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("using System;\r\nusing System.Collections.Generic;\r\nusing Blueprint41;\r\nusing Bluep" +
                    "rint41.Core;\r\nusing Blueprint41.Query;\r\n\r\nnamespace ");
            
            #line 13 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Settings.FullQueryNamespace));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n\tpublic partial class Node\r\n\t{\r\n");
            
            #line 17 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

	if(DALModel.IsVirtual)
    {

            
            #line default
            #line hidden
            this.Write("\t\t[Obsolete(\"This entity is virtual, consider making entity ");
            
            #line 21 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write(" concrete or use another entity as your starting point.\", true)]\r\n");
            
            #line 22 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

    }

            
            #line default
            #line hidden
            this.Write("\t\tpublic static ");
            
            #line 25 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node ");
            
            #line 25 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write(" { get { return new ");
            
            #line 25 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node(); } }\r\n\t}\r\n\r\n\tpublic partial class ");
            
            #line 28 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node : Blueprint41.Query.Node\r\n\t{\r\n        public override string Neo4jLabel\r\n   " +
                    "     {\r\n            get\r\n            {\r\n");
            
            #line 34 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

	if(DALModel.IsVirtual)
    {

            
            #line default
            #line hidden
            this.Write("                return null;\r\n");
            
            #line 39 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

    }
	else
    {

            
            #line default
            #line hidden
            this.Write("\t\t\t\treturn \"");
            
            #line 44 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Label.Name));
            
            #line default
            #line hidden
            this.Write("\";\r\n");
            
            #line 45 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

    }

            
            #line default
            #line hidden
            this.Write("            }\r\n        }\r\n\r\n\t\tinternal ");
            
            #line 51 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node() { }\r\n\t\tinternal ");
            
            #line 52 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node(");
            
            #line 52 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Alias alias, bool isReference = false)\r\n\t\t{\r\n\t\t\tNodeAlias = alias;\r\n\t\t\tIsReferenc" +
                    "e = isReference;\r\n\t\t}\r\n\t\tinternal ");
            
            #line 57 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node(RELATIONSHIP relationship, DirectionEnum direction) : base(relationship, dir" +
                    "ection) { }\r\n\r\n\t\tpublic ");
            
            #line 59 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node Alias(out ");
            
            #line 59 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Alias alias)\r\n\t\t{\r\n\t\t\talias = new ");
            
            #line 61 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Alias(this);\r\n            NodeAlias = alias;\r\n\t\t\treturn this;\r\n\t\t}\r\n\r\n");
            
            #line 66 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

	var inRelations =  Datastore.Relations.Where(item => DALModel.IsSelfOrSubclassOf(item.InEntity)).OrderBy(item => item.Name);
	var outRelations = Datastore.Relations.Where(item => DALModel.IsSelfOrSubclassOf(item.OutEntity)).OrderBy(item => item.Name);
	var anyRelations = Datastore.Relations.Where(item => DALModel.IsSelfOrSubclassOf(item.OutEntity) && item.InEntity == item.OutEntity).OrderBy(item => item.Name);

	if (inRelations.Any())
    {

            
            #line default
            #line hidden
            this.Write("\t\r\n\t\tpublic ");
            
            #line 74 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("In  In  { get { return new ");
            
            #line 74 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("In(this); } }\r\n\t\tpublic class ");
            
            #line 75 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("In\r\n\t\t{\r\n\t\t\tprivate ");
            
            #line 77 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node Parent;\r\n\t\t\tinternal ");
            
            #line 78 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("In(");
            
            #line 78 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node parent)\r\n\t\t\t{\r\n                Parent = parent;\r\n\t\t\t}\r\n");
            
            #line 82 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

		foreach (Relationship rel in inRelations)
		{

            
            #line default
            #line hidden
            this.Write("\t\t\tpublic IFromIn_");
            
            #line 86 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(rel.Name));
            
            #line default
            #line hidden
            this.Write("_REL ");
            
            #line 86 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(rel.Name));
            
            #line default
            #line hidden
            this.Write(" { get { return new ");
            
            #line 86 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(rel.Name));
            
            #line default
            #line hidden
            this.Write("_REL(Parent, DirectionEnum.In); } }\r\n");
            
            #line 87 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

		}

            
            #line default
            #line hidden
            this.Write("\r\n\t\t}\r\n");
            
            #line 92 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

    }

	if (outRelations.Any())
    {

            
            #line default
            #line hidden
            this.Write("\r\n\t\tpublic ");
            
            #line 99 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Out Out { get { return new ");
            
            #line 99 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Out(this); } }\r\n\t\tpublic class ");
            
            #line 100 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Out\r\n\t\t{\r\n\t\t\tprivate ");
            
            #line 102 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node Parent;\r\n\t\t\tinternal ");
            
            #line 103 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Out(");
            
            #line 103 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node parent)\r\n\t\t\t{\r\n                Parent = parent;\r\n\t\t\t}\r\n");
            
            #line 107 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

		foreach (Relationship rel in outRelations)
		{

            
            #line default
            #line hidden
            this.Write("\t\t\tpublic IFromOut_");
            
            #line 111 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(rel.Name));
            
            #line default
            #line hidden
            this.Write("_REL ");
            
            #line 111 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(rel.Name));
            
            #line default
            #line hidden
            this.Write(" { get { return new ");
            
            #line 111 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(rel.Name));
            
            #line default
            #line hidden
            this.Write("_REL(Parent, DirectionEnum.Out); } }\r\n");
            
            #line 112 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

		}

            
            #line default
            #line hidden
            this.Write("\t\t}\r\n");
            
            #line 116 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

    }

	if (anyRelations.Any())
    {

            
            #line default
            #line hidden
            this.Write("\r\n\t\tpublic ");
            
            #line 123 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Any Any { get { return new ");
            
            #line 123 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Any(this); } }\r\n\t\tpublic class ");
            
            #line 124 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Any\r\n\t\t{\r\n\t\t\tprivate ");
            
            #line 126 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node Parent;\r\n\t\t\tinternal ");
            
            #line 127 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Any(");
            
            #line 127 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node parent)\r\n\t\t\t{\r\n                Parent = parent;\r\n\t\t\t}\r\n");
            
            #line 131 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

		foreach (Relationship rel in anyRelations)
		{

            
            #line default
            #line hidden
            this.Write("\t\t\tpublic IFromAny_");
            
            #line 135 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(rel.Name));
            
            #line default
            #line hidden
            this.Write("_REL ");
            
            #line 135 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(rel.Name));
            
            #line default
            #line hidden
            this.Write(" { get { return new ");
            
            #line 135 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(rel.Name));
            
            #line default
            #line hidden
            this.Write("_REL(Parent, DirectionEnum.None); } }\r\n");
            
            #line 136 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

		}

            
            #line default
            #line hidden
            this.Write("\t\t}\r\n");
            
            #line 140 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

    }

            
            #line default
            #line hidden
            this.Write("\t}\r\n\r\n    public class ");
            
            #line 145 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Alias : AliasResult\r\n    {\r\n        internal ");
            
            #line 147 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Alias(");
            
            #line 147 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write(@"Node parent)
        {
			Node = parent;
        }

        public override IReadOnlyDictionary<string, FieldResult> AliasFields
        {
            get
            {
                if (m_AliasFields == null)
                {
                    m_AliasFields = ");
            
            #line 158 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(string.IsNullOrEmpty(DALModel.UnidentifiedProperties) ? "" : "new UnidentifiedPropertiesAliasDictionary("));
            
            #line default
            #line hidden
            this.Write("new Dictionary<string, FieldResult>()\r\n                    {\r\n");
            
            #line 160 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

	foreach (var property in DALModel.GetPropertiesOfBaseTypesAndSelf())
    {
		if (property.PropertyType != PropertyType.Attribute)
			continue;

            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t\t{ \"");
            
            #line 166 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write("\", new ");
            
            #line 166 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetResultType(property.SystemReturnType)));
            
            #line default
            #line hidden
            this.Write("(this, \"");
            
            #line 166 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write("\", ");
            
            #line 166 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Datastore.GetType().FullName));
            
            #line default
            #line hidden
            this.Write(".Model.Entities[\"");
            
            #line 166 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("\"], ");
            
            #line 166 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Datastore.GetType().FullName));
            
            #line default
            #line hidden
            this.Write(".Model.Entities[\"");
            
            #line 166 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Parent.Name));
            
            #line default
            #line hidden
            this.Write("\"].Properties[\"");
            
            #line 166 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write("\"]) },\r\n");
            
            #line 167 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

	}

            
            #line default
            #line hidden
            this.Write("\t\t\t\t\t}");
            
            #line 170 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(string.IsNullOrEmpty(DALModel.UnidentifiedProperties) ? "" : string.Concat(", ", Settings.FullCRUDNamespace, ".", DALModel.Name ,".Entity, this)")));
            
            #line default
            #line hidden
            this.Write(";\r\n\t\t\t\t}\r\n\t\t\t\treturn m_AliasFields;\r\n\t\t\t}\r\n\t\t}\r\n        private IReadOnlyDictiona" +
                    "ry<string, FieldResult> m_AliasFields = null;\r\n\r\n");
            
            #line 177 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

	if (inRelations.Any())
    {

            
            #line default
            #line hidden
            this.Write("        public ");
            
            #line 181 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node.");
            
            #line 181 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("In In { get { return new ");
            
            #line 181 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node.");
            
            #line 181 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("In(new ");
            
            #line 181 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node(this, true)); } }\r\n");
            
            #line 182 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

	}
	if (outRelations.Any())
    {

            
            #line default
            #line hidden
            this.Write("        public ");
            
            #line 187 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node.");
            
            #line 187 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Out Out { get { return new ");
            
            #line 187 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node.");
            
            #line 187 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Out(new ");
            
            #line 187 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node(this, true)); } }\r\n");
            
            #line 188 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

	}
	if (anyRelations.Any())
    {

            
            #line default
            #line hidden
            this.Write("        public ");
            
            #line 193 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node.");
            
            #line 193 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Any Any { get { return new ");
            
            #line 193 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node.");
            
            #line 193 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Any(new ");
            
            #line 193 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Node(this, true)); } }\r\n");
            
            #line 194 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

	}

            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 198 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

	foreach (var property in DALModel.GetPropertiesOfBaseTypesAndSelf())
    {
		if (property.PropertyType != PropertyType.Attribute)
			continue;


            
            #line default
            #line hidden
            this.Write("        public ");
            
            #line 205 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetResultType(property.SystemReturnType)));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 205 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t{\r\n\t\t\tget\r\n\t\t\t{\r\n\t\t\t\tif ((object)m_");
            
            #line 209 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write(" == null)\r\n\t\t\t\t\tm_");
            
            #line 210 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write(" = (");
            
            #line 210 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetResultType(property.SystemReturnType)));
            
            #line default
            #line hidden
            this.Write(")AliasFields[\"");
            
            #line 210 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write("\"];\r\n\r\n\t\t\t\treturn m_");
            
            #line 212 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write(";\r\n\t\t\t}\r\n\t\t} \r\n        private ");
            
            #line 215 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetResultType(property.SystemReturnType)));
            
            #line default
            #line hidden
            this.Write(" m_");
            
            #line 215 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write(" = null;\r\n");
            
            #line 216 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

    }

	if (!string.IsNullOrEmpty(DALModel.UnidentifiedProperties))
    {

            
            #line default
            #line hidden
            this.Write("        public UnidentifiedProperties ");
            
            #line 222 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.UnidentifiedProperties));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t{\r\n\t\t\tget\r\n\t\t\t{\r\n\t\t\t\tif ((object)m_");
            
            #line 226 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.UnidentifiedProperties));
            
            #line default
            #line hidden
            this.Write(" == null)\r\n\t\t\t\t\tm_");
            
            #line 227 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.UnidentifiedProperties));
            
            #line default
            #line hidden
            this.Write(" = new UnidentifiedProperties(this, ");
            
            #line 227 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Datastore.GetType().FullName));
            
            #line default
            #line hidden
            this.Write(".Model.Entities[\"");
            
            #line 227 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("\"]);\r\n\r\n\t\t\t\treturn m_");
            
            #line 229 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.UnidentifiedProperties));
            
            #line default
            #line hidden
            this.Write(";\r\n\t\t\t}\r\n\t\t}\r\n\t\tprivate UnidentifiedProperties m_");
            
            #line 232 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.UnidentifiedProperties));
            
            #line default
            #line hidden
            this.Write(" = null;\r\n\r\n        public class UnidentifiedProperties\r\n        {\r\n            i" +
                    "nternal UnidentifiedProperties(");
            
            #line 236 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("Alias alias, Entity entity)\r\n            {\r\n                Alias = alias;\r\n     " +
                    "           Entity = entity;\r\n            }\r\n            private ");
            
            #line 241 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write(@"Alias Alias;
            private Entity Entity;

            public MiscResult Get(string fieldName) { return new MiscResult(Alias, fieldName, Entity, null); }
            public MiscResult this[string fieldName] { get { return Get(fieldName); } }

			public MiscResult Get(FieldResult result, bool withCoalesce = false, Type type = null)
            {
				if (withCoalesce)
					return new MiscResult(""{0}[COALESCE({1}, '')]"", new object[] { Alias, result }, type ?? typeof(object));
				else
					return new MiscResult(""{0}[{1}]"", new object[] { Alias, result }, type ?? typeof(object));
            }
        }
");
            
            #line 255 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"

    }

            
            #line default
            #line hidden
            this.Write("    }\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 260 "C:\_Xirqlz\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Node.tt"


	private string GetResultType(Type type)
	{
		switch (type.Name)
		{
			case "Boolean":
				return "BooleanResult";
			case "Int16":
			case "Int32":
			case "Int64":
				return "NumericResult";
			case "Single":
			case "Double":
				return "FloatResult";
			case "Guid":
			case "String":
				return "StringResult";
			case "DateTime":
				return "DateTimeResult";
			case "List`1":
				if(type.GenericTypeArguments[0] == typeof(string))
					return "StringListResult";
				else
					return "ListResult";
			default:
				return "MiscResult";
		}
	}

        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
}
