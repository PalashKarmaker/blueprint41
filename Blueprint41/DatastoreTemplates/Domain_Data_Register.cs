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
    using System.Collections.Generic;
    using Blueprint41.Core;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\Glenn\source\repos\circles-arrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Register.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class Domain_Data_Register : GeneratorBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("using System;\r\nusing Blueprint41.Core;\r\n\r\nnamespace ");
            
            #line 8 "C:\Users\Glenn\source\repos\circles-arrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Register.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Settings.FullCRUDNamespace));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n\tinternal class Register\r\n\t{\r\n\t\tprivate static bool isInitialized = false;\r\n" +
                    "\r\n\t\tpublic static void Types()\r\n\t\t{\r\n\t\t\tif (");
            
            #line 16 "C:\Users\Glenn\source\repos\circles-arrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Register.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Datastore.GetType().FullName));
            
            #line default
            #line hidden
            this.Write(".Model.TypesRegistered)\r\n\t\t\t\treturn;\r\n\r\n\t\t\tlock (typeof(Register))\r\n\t\t\t{\r\n\t\t\t\tif " +
                    "(");
            
            #line 21 "C:\Users\Glenn\source\repos\circles-arrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Register.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Datastore.GetType().FullName));
            
            #line default
            #line hidden
            this.Write(".Model.TypesRegistered)\r\n\t\t\t\t\treturn;\r\n\r\n");
            
            #line 24 "C:\Users\Glenn\source\repos\circles-arrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Register.tt"

foreach (var DALModel in Datastore.Entities.OrderBy(item => item.Name))
{	

            
            #line default
            #line hidden
            this.Write("\t\t\t\t((ISetRuntimeType)");
            
            #line 28 "C:\Users\Glenn\source\repos\circles-arrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Register.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Datastore.GetType().FullName));
            
            #line default
            #line hidden
            this.Write(".Model.Entities[\"");
            
            #line 28 "C:\Users\Glenn\source\repos\circles-arrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Register.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("\"]).SetRuntimeTypes(typeof(");
            
            #line 28 "C:\Users\Glenn\source\repos\circles-arrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Register.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.ClassName));
            
            #line default
            #line hidden
            this.Write("), typeof(");
            
            #line 28 "C:\Users\Glenn\source\repos\circles-arrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Register.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DALModel.Name));
            
            #line default
            #line hidden
            this.Write("));\r\n");
            
            #line 29 "C:\Users\Glenn\source\repos\circles-arrows\blueprint41\Blueprint41\DatastoreTemplates\Domain_Data_Register.tt"

}

            
            #line default
            #line hidden
            this.Write("\t\t\t}\r\n\t\t}\r\n\t}\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
