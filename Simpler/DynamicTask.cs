using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class DynamicTask : Task
    {
        public virtual dynamic In { get; set; }

        public virtual dynamic Out { get; set; }

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