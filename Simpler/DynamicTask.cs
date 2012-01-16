using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class DynamicTask : Task
    {
        public virtual dynamic Inputs { get; set; }

        public virtual dynamic Outputs { get; set; }

        public virtual DynamicTask SetInputs(dynamic inputs)
        {
            Inputs = inputs;
            return this;
        }

        public virtual dynamic GetOutputs()
        {
            Execute();
            return Outputs;
        }
    }
}