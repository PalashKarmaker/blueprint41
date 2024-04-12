using System;
using System.Collections.Generic;
using System.Text;

namespace Blueprint41.DatastoreTemplates;

public class GeneratorResult
{
    public Dictionary<string, string> EntityResult { get; set; }
    public Dictionary<string, string> RelationshipResult { get; set; }
    public Dictionary<string, string> NodeResult { get; set; }

    public GeneratorResult()
    {
        EntityResult = [];
        RelationshipResult = [];
        NodeResult = [];
    }
}
