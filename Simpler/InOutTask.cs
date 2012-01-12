﻿using Simpler.Injection;

namespace Simpler
{
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

        public InOutTask<TInputs, TOutputs> DoItUsing(TInputs inputs)
        {
            Inputs = inputs;
            Execute();
            return this;
        }
    }
}
