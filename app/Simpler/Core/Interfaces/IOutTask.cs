namespace Simpler.Core.Interfaces
{
    public interface IOutTask<TOut> : ITask
    {
        TOut Out { get; set; }
    }
}