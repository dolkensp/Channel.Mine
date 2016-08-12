using System;

namespace Channel.Mine.Framework.Abstraction
{
    public delegate Boolean PipelineDelegate<T>(T item);

    public abstract class DynamicPipeline<T>
    {
        #region Types

        private class AndFilter<U> : DynamicPipeline<U>
        {
            public DynamicPipeline<U> Filter1 { get; set; }
            public DynamicPipeline<U> Filter2 { get; set; }

            public override PipelineDelegate<U> Process
            {
                get
                {
                    if (this.Filter1 == null)
                        return this.Filter2.Process;
                    else if (this.Filter2 == null)
                        return this.Filter1.Process;

                    return (item) => this.Filter1.Process(item) && this.Filter2.Process(item);
                }
            }
        }

        private class OrFilter<U> : DynamicPipeline<U>
        {
            public DynamicPipeline<U> Filter1 { get; set; }
            public DynamicPipeline<U> Filter2 { get; set; }

            public override PipelineDelegate<U> Process
            {
                get
                {
                    if (this.Filter1 == null)
                        return this.Filter2.Process;
                    else if (this.Filter2 == null)
                        return this.Filter1.Process;

                    return (item) => this.Filter1.Process(item) || this.Filter2.Process(item);
                }
            }
        }

        #endregion

        #region Properties

        public abstract PipelineDelegate<T> Process { get; }

        #endregion

        #region Operators

        public static DynamicPipeline<T> operator &(DynamicPipeline<T> filter1, DynamicPipeline<T> filter2)
        {
            return new AndFilter<T>
            {
                Filter1 = filter1,
                Filter2 = filter2
            };
        }

        public static DynamicPipeline<T> operator |(DynamicPipeline<T> filter1, DynamicPipeline<T> filter2)
        {
            return new OrFilter<T>
            {
                Filter1 = filter1,
                Filter2 = filter2
            };
        }

        #endregion
    }
}
