namespace Simpler.Tests.Core.Mocks
{
    [MockFirst, MockSecond]
    public class MockTaskWithAttributesThatThrows : MockTaskWithAttributes
    {
        public override void Execute()
        {
            base.Execute();
            throw new MockException();
        }
    }
}