using System;

namespace Channel.Mine.Framework.Abstraction
{
    public abstract class BaseFilter<T> : Abstraction.DynamicPipeline<T>
    {
        public abstract Boolean DoFilter(T item);

        public override Abstraction.PipelineDelegate<T> Process
        {
            get
            {
                return (item) => this.DoFilter(item);
            }
        }
    }
}
