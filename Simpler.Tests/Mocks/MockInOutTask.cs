using System;

namespace Simpler.Tests.Mocks
{
    public class MockInOutTask : InOutTask<MockObject, MockComplexObject>
    {
        public new MockObject In { get { return base.In; } set { base.In = value; } }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
