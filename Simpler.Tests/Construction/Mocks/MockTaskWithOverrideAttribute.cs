namespace Simpler.Tests.Construction.Mocks
{
    [Override]
    public class MockTaskWithOverrideAttribute : Task
    {
        public bool OverrideWasCalled;
        public bool WasExecuted;

        public override void Execute()
        {
            WasExecuted = true;
        }
    }
}