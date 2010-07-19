using System.Data;
using Simpler.Data.Tasks;

namespace Simpler.Data.Interfaces
{
    public interface IBuildParametersUsing<T>
    {
        IDbCommand CommandWithParameters { get; set; }
        T ObjectWithValues { get; set; }
        IFindParametersInCommandText FindParametersInCommandText { set; }
        void Execute();
    }
}