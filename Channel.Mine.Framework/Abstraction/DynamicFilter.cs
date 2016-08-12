using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Channel.Mine.Framework.Abstraction
{
    public delegate Boolean FilterDelegate<T>(T item);

    public sealed class DynamicFilter<T> : BaseFilter<T>
    {
        public FilterDelegate<T> Filter { get; private set; }

        public DynamicFilter(FilterDelegate<T> filter) { this.Filter = filter; }

        public override Boolean DoFilter(T item) { return this.Filter(item); }
    }
}
