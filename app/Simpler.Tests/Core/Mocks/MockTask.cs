using System;

namespace Simpler.Tests.Core.Mocks
{
    public class MockTask : T
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}