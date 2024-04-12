using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint41;

public class InstanceCollection : IEnumerable<Instance>
{
    internal InstanceCollection(Entity parent)
         : base()
    {
        Parent = parent;
    }

    public Entity Parent { get; private set; }

    protected List<Instance> collection = [];
    internal void Add(Instance value) => collection.Add(value);

    public Instance First(Predicate<dynamic> predicate) => collection.First(item => predicate.Invoke(item.Values));
    public Instance? FirstOrDefault(Predicate<dynamic> predicate) => collection.Find(item => predicate.Invoke(item.Values));

    #region IEnumerable<Node>

    IEnumerator IEnumerable.GetEnumerator() => collection.GetEnumerator();

    IEnumerator<Instance> IEnumerable<Instance>.GetEnumerator() => collection.GetEnumerator();

    #endregion
}
