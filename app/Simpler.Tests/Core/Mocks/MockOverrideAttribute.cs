namespace Simpler.Tests.Core.Mocks
{
    public class MockOverrideAttribute : OverrideAttribute
    {
        public override void ExecuteOverride(Task task)
        {
            var taskWithOverrideAttribute = (MockTaskWithOverrideAttribute) task;
            if (taskWithOverrideAttribute.OverrideShouldProceed)
            {
                taskWithOverrideAttribute.Execute();
            }
        }
    }
}