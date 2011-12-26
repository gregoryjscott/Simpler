using System;

namespace Simpler.Tests.Mocks
{
    public class MockTaskTITO : Task<MockObject, MockComplexObject>
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
