namespace Simpler.Data.Interfaces
{
    public interface IFindParametersInCommandText
    {
        string CommandText { get; set; }
        string[] ParameterNames { get; }
        void Execute();
    }
}