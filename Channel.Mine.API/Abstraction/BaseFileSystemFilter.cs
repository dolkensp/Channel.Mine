using System;

namespace Channel.Mine.API.Abstraction
{
    public abstract class BaseFileSystemFilter<T> : Framework.Abstraction.BaseFilter<T> where T : Framework.Abstraction.BaseMedia
    {
        public String Query { get; internal set; }

        public BaseFileSystemFilter(String query)
        {
            this.Query = query;
        }

        public override Boolean DoFilter(T item)
        {
            return item.File.FullName.IndexOf(this.Query, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }
    }
}