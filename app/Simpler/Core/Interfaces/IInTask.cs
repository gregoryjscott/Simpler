namespace Simpler.Core.Interfaces
{
    public interface IInTask<TIn> : ITask
    {
        TIn In { get; set; }
    }
}