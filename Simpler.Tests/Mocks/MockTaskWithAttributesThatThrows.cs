namespace Simpler.Tests.Mocks
{
    [First, Second]
    public class MockTaskWithAttributesThatThrows : MockTaskWithAttributes
    {
        public override void Execute()
        {
            base.Execute();
            throw new TestException();
        }
    }
}