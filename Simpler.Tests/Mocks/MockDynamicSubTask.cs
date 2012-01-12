namespace Simpler.Tests.Mocks
{
    public class MockDynamicSubTask : DynamicTask
    {
        public override void Execute()
        {
            Outputs = new { SomeOutput = 9 };
        }
    }
}
