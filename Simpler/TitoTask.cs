using System;
using Simpler.Injection;

namespace Simpler
{
    /// <summary>
    /// TitoTask stands for "type inputs, type outputs, task" meaning that generic types are
    /// provided to this Task subclass that will be used to produce strongly typed inputs and outputs
    /// made available through the InputsModels and OutputsModels properties.
    /// </summary>
    /// <typeparam name="TI">The type for InputsModel.</typeparam>
    /// <typeparam name="TO">The type for OutputsModel.</typeparam>
    [InjectSubTasks]
    public abstract class TitoTask<TI, TO> : Task
    {
        public override dynamic Inputs
        {
            get { return InputsModel; }
            set
            {
                InputsModel = Mapper.Map<TI>(value);
            }
        }
        public override dynamic Outputs
        {
            get { return OutputsModel; }
            set
            {
                OutputsModel = Mapper.Map<TO>(value);
            }
        }

        public virtual TI InputsModel { get; set; }
        public virtual TO OutputsModel { get; set; }

        /// <summary>
        /// Shorthand method for setting the InputsModel, executing, and receiving the outputs in one line of code.
        /// </summary>
        /// <param name="inputsModel">Used to set InputsModel prior to execution.</param>
        /// <returns>The TitoTask that allows for .Outputs or .OutputsModel.</returns>
        public TitoTask<TI, TO> Execute(TI inputsModel)
        {
            InputsModel = inputsModel;
            Execute();
            return this;
        }
    }
}
