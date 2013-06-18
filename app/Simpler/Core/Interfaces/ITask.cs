namespace Simpler.Core.Interfaces
{
    public interface ITask
    {
        string Name { get; }
        Stats Stats { get; set; }
        void Execute();
    }
}