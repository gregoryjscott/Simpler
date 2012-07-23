namespace Simpler.Tests.Core.Mocks
{
    [MockFirst, MockSecond]
    public class MockTaskWithAttributesThatThrows : MockTaskWithAttributes
    {
        public override void Run()
        {
            base.Run();
            throw new MockException();
        }
    }
}