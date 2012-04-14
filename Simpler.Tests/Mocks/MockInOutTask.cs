using System;

namespace Simpler.Tests.Mocks
{
    public class MockInOutJob : InOutJob<MockObject, MockComplexObject>
    {
        public new MockObject In { get { return base._In; } set { base._In = value; } }

        public override void Run()
        {
            throw new NotImplementedException();
        }
    }
}
