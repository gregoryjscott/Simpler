namespace Simpler.Tests.Mocks
{
    [MockFirst, MockSecond]
    public class MockJobWithAttributesThatThrows : MockJobWithAttributes
    {
        public override void Run()
        {
            base.Run();
            throw new MockException();
        }
    }
}