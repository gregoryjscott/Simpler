namespace Simpler.Tests.Mocks
{
    public class MockSubTaskUsingDynamicProperties : Task
    {
        public override void Execute()
        {
            Outputs = new { SomeOutput = 9 };
        }
    }
}
