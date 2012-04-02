using System;

namespace Simpler.Tests.Mocks
{
    public class MockInOutTask : InOutTask<MockObject, MockComplexObject>
    {
        public new MockObject In { get { return base.Input; } set { base.Input = value; } }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
