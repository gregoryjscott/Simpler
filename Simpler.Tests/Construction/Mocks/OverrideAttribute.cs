using Simpler.Construction;

namespace Simpler.Tests.Construction.Mocks
{
    public class OverrideAttribute : ExecutionOverrideAttribute
    {
        public override void ExecutionOverride(Task task)
        {
            ((MockTaskWithOverrideAttribute) task).OverrideWasCalled = true;
        }
    }
}