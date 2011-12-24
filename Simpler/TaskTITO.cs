using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class Task<TI, TO> : Task
    {
        dynamic _inputs;
        dynamic _outputs;
        TI _inputsModel;
        TO _outputsModel;

        public override dynamic Inputs
        {
            get { return _inputs; }
            set
            {
                _inputs = value;
                Mapper.DynamicMap<TI>(_inputs, _inputsModel);
            }
        }
        public override dynamic Outputs
        {
            get { return _outputs; }
            set
            {
                _outputs = value;
                Mapper.DynamicMap<TO>(_outputs, _outputsModel);
            }
        }
        public TI InputsModel
        {
            get { return _inputsModel; }
            set
            {
                _inputsModel = value;
                _inputs = _inputsModel;
            }
        }
        public TO OutputsModel
        {
            get { return _outputsModel; }
            set
            {
                _outputsModel = value;
                _outputs = _outputsModel;
            }
        }
    }
}
