namespace Simpler.Tests.Mocks
{
    public class MockDynamicSubTask : DynamicTask
    {
        public override void Execute()
        {
            Out = new { SomeOutput = 9 };
        }
    }
}
