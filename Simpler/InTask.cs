using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class InTask<TInputs> : Task
    {
        public virtual TInputs In { get; set; }

        public virtual InTask<TInputs> SetInputs(object inputs)
        {
            In = inputs == null
                         ? default(TInputs)
                         : Mapper.Map<TInputs>(inputs);

            return this;
        }

        public virtual InTask<TInputs> SetInputs(TInputs inputs)
        {
            In = inputs;
            return this;
        }
    }
}