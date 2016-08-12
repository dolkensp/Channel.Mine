
namespace Channel.Mine.Framework.Abstraction
{
    public abstract class BaseAction<T> : Abstraction.DynamicPipeline<T>
    {
        public abstract void DoAction(T item);

        public override Abstraction.PipelineDelegate<T> Process
        {
            get
            {
                return (item) =>
                {
                    this.DoAction(item);
                    return true;
                };
            }
        }
    }
}
