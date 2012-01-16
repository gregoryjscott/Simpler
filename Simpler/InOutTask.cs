using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class InOutTask<TInputs, TOutputs> : Task
    {
        public virtual TInputs Inputs { get; set; }

        public virtual TOutputs Outputs { get; set; }

        public virtual InOutTask<TInputs, TOutputs> SetInputs(object inputs)
        {
            Inputs = inputs == null
                         ? default(TInputs)
                         : Mapper.Map<TInputs>(inputs);
            return this;
        }

        public virtual InOutTask<TInputs, TOutputs> SetInputs(TInputs inputs)
        {
            Inputs = inputs;
            return this;
        }

        public virtual TOutputs GetOutputs()
        {
            Execute();
            return Outputs;
        }
    }
}
