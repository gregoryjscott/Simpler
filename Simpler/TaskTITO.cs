using System;
using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class Task<TI, TO> : Task where TI : class where TO : class
    {
        public override dynamic Inputs
        {
            get { return InputsModel; }
            set
            {
                InputsModel = (TI)Activator.CreateInstance(typeof(TI));
                Mapper.Map<TI>(value, InputsModel);
            }
        }
        public override dynamic Outputs
        {
            get { return OutputsModel; }
            set
            {
                OutputsModel = (TO)Activator.CreateInstance(typeof(TO));
                Mapper.Map<TO>(value, OutputsModel);
            }
        }

        public virtual TI InputsModel { get; set; }
        public virtual TO OutputsModel { get; set; }
    }
}
