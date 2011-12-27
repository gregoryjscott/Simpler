using Simpler.Injection;

namespace Simpler
{
    /// <summary>
    /// Task that accepts Type In and Type Out, meaning that generic types that are provided
    /// to this Task subclass that will be used to produce strongly typed inputs and outputs
    /// made available through the InputsModels and OutputsModels properties.
    /// </summary>
    /// <typeparam name="TI">The type for InputsModel.</typeparam>
    /// <typeparam name="TO">The type for OutputsModel.</typeparam>
    [InjectSubTasks]
    public abstract class Task<TI, TO> : Task
    {
        public override dynamic Inputs
        {
            get { return InputsModel; }
            set
            {
                InputsModel = value == null
                                  ? default(TI)
                                  : Mapper.Map<TI>(value);
            }
        }
        public override dynamic Outputs
        {
            get { return OutputsModel; }
            set
            {
                OutputsModel = value == null
                                  ? default(TO)
                                  : Mapper.Map<TO>(value);
            }
        }

        /// <summary>
        /// Typed container for the data the task needs to execute.
        /// </summary>
        public virtual TI InputsModel { get; set; }

        /// <summary>
        /// Typed container for the data the task produces.
        /// </summary>
        public virtual TO OutputsModel { get; set; }

        /// <summary>
        /// Shorthand method for setting the InputsModel, executing, and receiving the outputs in one line of code.
        /// </summary>
        /// <param name="inputsModel">Used to set InputsModel prior to execution.</param>
        /// <returns>The TaskTITO that allows for .Outputs or .OutputsModel.</returns>
        public Task<TI, TO> Execute(TI inputsModel)
        {
            InputsModel = inputsModel;
            Execute();
            return this;
        }
    }
}
