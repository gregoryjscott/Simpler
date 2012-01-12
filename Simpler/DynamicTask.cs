namespace Simpler
{
    public abstract class DynamicTask : Task
    {
        public virtual dynamic Inputs { get; set; }

        public virtual dynamic Outputs { get; set; }

        public DynamicTask Execute(dynamic inputs)
        {
            Inputs = inputs;
            Execute();
            return this;
        }
    }
}