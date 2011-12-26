using System;

namespace Simpler.Tests.Mocks
{
    public class MockTaskTITOUsingPrimitives : Task<int, string>
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
