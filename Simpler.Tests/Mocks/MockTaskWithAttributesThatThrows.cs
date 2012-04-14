namespace Simpler.Tests.Mocks
{
    [First, Second]
    public class MockJobWithAttributesThatThrows : MockJobWithAttributes
    {
        public override void Execute()
        {
            base.Execute();
            throw new TestException();
        }
    }
}