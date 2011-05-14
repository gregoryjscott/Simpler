using System;

namespace Simpler.Tests.Construction.Mocks
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