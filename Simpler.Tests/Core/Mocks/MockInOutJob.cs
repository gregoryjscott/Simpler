using System;

namespace Simpler.Tests.Core.Mocks
{
    public class MockInOutJob : InOutJob<MockObject, MockComplexObject>
    {
        public new MockObject In { get { return base.In; } set { base.In = value; } }

        public override void Run()
        {
            Out.MockObject = In;
        }
    }
}
