using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class InOutTask<TInputs, TOutputs> : Task
    {
        protected TInputs In { get; set; }

        protected TOutputs Out { get; set; }

        public virtual InOutTask<TInputs, TOutputs> SetInputs(object inputs)
        {
            In = inputs == null
                         ? default(TInputs)
                         : Mapper.Map<TInputs>(inputs);
            return this;
        }

        public virtual InOutTask<TInputs, TOutputs> SetInputs(TInputs inputs)
        {
            In = inputs;
            return this;
        }

        public virtual TOutputs GetOutputs()
        {
            Execute();
            return Out;
        }
    }
}