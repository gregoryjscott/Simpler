using System;

namespace Simpler.Tests.Mocks
{
    public class MockInOutTaskUsingPrimitives : InOutTask<int, string>
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
