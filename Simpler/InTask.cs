using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class InTask<TInputs> : Task
    {
        public virtual TInputs Inputs { get; set; }

        public virtual InTask<TInputs> SetInputs(object inputs)
        {
            Inputs = inputs == null
                         ? default(TInputs)
                         : Mapper.Map<TInputs>(inputs);

            return this;
        }

        public virtual InTask<TInputs> SetInputs(TInputs inputs)
        {
            Inputs = inputs;
            return this;
        }
    }
}
