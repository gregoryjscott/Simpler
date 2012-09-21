namespace Simpler.Tests.Core.Mocks
{
    [MockFirst, MockSecond]
    public class MockTaskThatThrowsWithAttributes : MockTaskWithAttributes
    {
        public override void Execute()
        {
            base.Execute();
            throw new MockException();
        }
    }
}