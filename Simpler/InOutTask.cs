using Simpler.Injection;

namespace Simpler
{
    /// <summary>
    /// Task that accepts a model in and a model out.  The Inputs property
    /// will be type of the given TInputs, the Outputs property will be of 
    /// type TOutputs.
    /// </summary>
    /// <typeparam name="TInputs">The type of Inputs.</typeparam>
    /// <typeparam name="TOutputs">The type of Outputs.</typeparam>
    [InjectSubTasks]
    public abstract class InOutTask<TInputs, TOutputs> : Task
    {
        public new virtual TInputs Inputs { get; set; }

        public new virtual TOutputs Outputs { get; set; }

        public virtual void SetInputs(dynamic inputs)
        {
            Inputs = inputs == null
                         ? default(TInputs)
                         : Mapper.Map<TInputs>(inputs);
        }

        public virtual dynamic GetOutputs()
        {
            return Outputs;
        }

        public InOutTask<TInputs, TOutputs> Execute(TInputs inputs)
        {
            Inputs = inputs;
            Execute();
            return this;
        }
    }
}
