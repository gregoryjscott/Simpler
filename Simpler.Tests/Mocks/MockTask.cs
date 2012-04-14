using System;

namespace Simpler.Tests.Mocks
{
    public class MockJob : Job
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}