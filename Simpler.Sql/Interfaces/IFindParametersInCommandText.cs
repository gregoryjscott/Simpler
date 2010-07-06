namespace Simpler.Sql.Tasks
{
    public interface IFindParametersInCommandText
    {
        string CommandText { get; set; }
        string[] ParameterNames { get; }
        void Execute();
    }
}