using System.Data;

namespace Simpler.Sql.Tasks
{
    public interface IBuildParametersUsing<T>
    {
        IDbCommand DbCommand { get; set; }
        T Object { get; set; }
        IFindParametersInCommandText FindParametersInCommandText { set; }
        void Execute();
    }
}