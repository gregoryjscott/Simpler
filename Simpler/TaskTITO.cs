using System;
using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class Task<TI, TO> : Task where TI : class where TO : class
    {

        TI _inputsModel;
        TO _outputsModel;

        public override dynamic Inputs
        {
            get { return _inputsModel; }
            set
            {
                _inputsModel = (TI)Activator.CreateInstance(typeof(TI));
                Mapper.DynamicMap<TI>(value, _inputsModel);
            }
        }
        public override dynamic Outputs
        {
            get { return _outputsModel; }
            set
            {
                _outputsModel = (TO)Activator.CreateInstance(typeof(TO));
                Mapper.DynamicMap<TO>(value, _outputsModel);
            }
        }
        public TI InputsModel
        {
            get { return _inputsModel; }
            set
            {
                _inputsModel = value;
            }
        }
        public TO OutputsModel
        {
            get { return _outputsModel; }
            set
            {
                _outputsModel = value;
            }
        }
    }
}
