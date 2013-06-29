namespace Simpler.Tests.Core.Mocks
{
    [MockFirst, MockSecond]
    public class MockSimpleTaskThatThrowsWithAttributes : MockSimpleTaskWithAttributes
    {
        public override void Execute()
        {
            base.Execute();
            throw new MockException();
        }
    }
}