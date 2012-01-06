using System;

namespace Simpler.Tests.Mocks
{
    public class MockInOutTask : InOutTask<MockObject, MockComplexObject>
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
