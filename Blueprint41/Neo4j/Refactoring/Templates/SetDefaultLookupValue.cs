﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Blueprint41.Neo4j.Refactoring.Templates
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Blueprint41;
    using Blueprint41.Dynamic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\_CirclesArrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\SetDefaultLookupValue.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    internal partial class SetDefaultLookupValue : SetDefaultLookupValueBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("\n");
            this.Write("\n");
            this.Write("\n");
            this.Write("\n");
            this.Write("\n");
            this.Write("\n");
            this.Write("\n");
            
            #line 1 "C:\_CirclesArrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\SetDefaultLookupValue.tt"


    Log("	executing {0} -> Set Default Lookup Value for {1}.{2}", this.GetType().Name, Property.Parent.Name, Property.Name);

            
            #line default
            #line hidden
            this.Write("\nMATCH (in:");
            
            #line 1 "C:\_CirclesArrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\SetDefaultLookupValue.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Property.Parent.Label.Name));
            
            #line default
            #line hidden
            this.Write(")\nMATCH (target:");
            
            #line 1 "C:\_CirclesArrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\SetDefaultLookupValue.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Property.ForeignEntity.Label.Name));
            
            #line default
            #line hidden
            this.Write(" { ");
            
            #line 1 "C:\_CirclesArrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\SetDefaultLookupValue.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Property.ForeignEntity.Key.Name));
            
            #line default
            #line hidden
            this.Write(" : \'");
            
            #line 1 "C:\_CirclesArrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\SetDefaultLookupValue.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Value));
            
            #line default
            #line hidden
            this.Write("\'})\nOPTIONAL MATCH (in)-[rel:");
            
            #line 1 "C:\_CirclesArrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\SetDefaultLookupValue.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Property.Relationship.Neo4JRelationshipType));
            
            #line default
            #line hidden
            this.Write("]-(out:");
            
            #line 1 "C:\_CirclesArrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\SetDefaultLookupValue.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Property.ForeignEntity.Label.Name));
            
            #line default
            #line hidden
            this.Write(")\nWITH in, count(out) as count, target\nWHERE count = 0\nWITH in, target LIMIT 1000" +
                    "0\nMERGE (in)-[rel:");
            
            #line 1 "C:\_CirclesArrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\SetDefaultLookupValue.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Property.Relationship.Neo4JRelationshipType));
            
            #line default
            #line hidden
            this.Write("]-(target)\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
