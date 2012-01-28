using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class DynamicTask : Task
    {
        protected dynamic In { get; set; }

        protected dynamic Out { get; set; }

        public virtual DynamicTask SetIns(object ins)
        {
            In = ins;
            return this;
        }

        public virtual dynamic GetOuts()
        {
            Execute();
            return Out;
        }
    }
}