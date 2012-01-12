using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class InTask<TInputs> : Task
    {
        public virtual TInputs Inputs { get; set; }

        public virtual void SetInputs(dynamic inputs)
        {
            Inputs = inputs == null
                         ? default(TInputs)
                         : Mapper.Map<TInputs>(inputs);
        }

        public virtual void DoItUsing(dynamic inputs)
        {
            SetInputs(inputs);
            Execute();
        }
    }
}
