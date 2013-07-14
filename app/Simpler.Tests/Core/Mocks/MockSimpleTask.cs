using System;

namespace Simpler.Tests.Core.Mocks
{
    public class MockSimpleTask : SimpleTask
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}