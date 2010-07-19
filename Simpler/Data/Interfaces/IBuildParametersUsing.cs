using System.Data;

namespace Simpler.Data.Tasks
{
    public interface IBuildParametersUsing<T>
    {
        IDbCommand CommandWithParameters { get; set; }
        T ObjectWithValues { get; set; }
        IFindParametersInCommandText FindParametersInCommandText { set; }
        void Execute();
    }
}