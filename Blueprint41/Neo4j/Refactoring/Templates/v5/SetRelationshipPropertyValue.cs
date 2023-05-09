﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Blueprint41.Neo4j.Refactoring.Templates.v5
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Blueprint41;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\Glenn\source\repos\circles-arrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\v5\SetRelationshipPropertyValue.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    internal partial class SetRelationshipPropertyValue : SetRelationshipPropertyValueBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            
            #line 7 "C:\Users\Glenn\source\repos\circles-arrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\v5\SetRelationshipPropertyValue.tt"


    string relationship = string.IsNullOrWhiteSpace(RelationshipType) ? "All Relationships" : RelationshipType;
    Log("	executing {0} -> SetRelationshipPropertyValue of  relationship {1} for property {2} = {3}", this.GetType().Name, relationship, Property, Value);
    string type = string.IsNullOrWhiteSpace(RelationshipType) ? "r" : "r:" + RelationshipType;

    OutputParameters.Add(Property, Value);

            
            #line default
            #line hidden
            this.Write("MATCH ()-[");
            
            #line 15 "C:\Users\Glenn\source\repos\circles-arrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\v5\SetRelationshipPropertyValue.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(type));
            
            #line default
            #line hidden
            this.Write("]->() WHERE r.");
            
            #line 15 "C:\Users\Glenn\source\repos\circles-arrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\v5\SetRelationshipPropertyValue.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Property));
            
            #line default
            #line hidden
            this.Write(" IS NULL  WITH r LIMIT 10000 SET r.");
            
            #line 15 "C:\Users\Glenn\source\repos\circles-arrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\v5\SetRelationshipPropertyValue.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Property));
            
            #line default
            #line hidden
            this.Write(" = {");
            
            #line 15 "C:\Users\Glenn\source\repos\circles-arrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\v5\SetRelationshipPropertyValue.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Property));
            
            #line default
            #line hidden
            this.Write("}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
