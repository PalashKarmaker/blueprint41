﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Blueprint41.Neo4j.Refactoring.Templates;

using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using Blueprint41;
using System;

/// <summary>
/// Class to produce the template output
/// </summary>

#line 1 "C:\_CirclesArrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\RenameRelationship.tt"
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
internal partial class RenameRelationship : RenameRelationshipBase
{
#line hidden
    /// <summary>
    /// Create the template output
    /// </summary>
    public override string TransformText()
    {

#line 7 "C:\_CirclesArrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\RenameRelationship.tt"


        Log("	executing {0} -> Rename relationship from {1} to {2}", this.GetType().Name, Relationship.Name, NewName);

        
        #line default
        #line hidden
        this.Write("MATCH (in:");
        
        #line 11 "C:\_CirclesArrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\RenameRelationship.tt"
        this.Write(this.ToStringHelper.ToStringWithCulture(Relationship.InEntity.Label.Name));
        
        #line default
        #line hidden
        this.Write(")-[rel:");
        
        #line 11 "C:\_CirclesArrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\RenameRelationship.tt"
        this.Write(this.ToStringHelper.ToStringWithCulture(OldName));
        
        #line default
        #line hidden
        this.Write("]-(out:");
        
        #line 11 "C:\_CirclesArrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\RenameRelationship.tt"
        this.Write(this.ToStringHelper.ToStringWithCulture(Relationship.OutEntity.Label.Name));
        
        #line default
        #line hidden
        this.Write(")\r\nWITH in, rel, out LIMIT 10000\r\nMERGE (in)-[newRelationship:");
        
        #line 13 "C:\_CirclesArrows\blueprint41\Blueprint41\Neo4j\Refactoring\Templates\RenameRelationship.tt"
        this.Write(this.ToStringHelper.ToStringWithCulture(NewName));
        
        #line default
        #line hidden
        this.Write("]->(out) ON CREATE SET newRelationship += rel WITH rel DELETE rel\r\n");
        return this.GenerationEnvironment.ToString();
    }
}

#line default
#line hidden

