using System;

namespace Simpler.Tests.Mocks
{
    public class MockTitoTask : TitoTask<MockObject, MockComplexObject>
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
