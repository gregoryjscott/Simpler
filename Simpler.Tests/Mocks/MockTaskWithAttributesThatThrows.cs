namespace Simpler.Tests.Mocks
{
    [First, Second]
    public class MockJobWithAttributesThatThrows : MockJobWithAttributes
    {
        public override void Run()
        {
            base.Run();
            throw new TestException();
        }
    }
}