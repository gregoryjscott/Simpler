using System;

namespace Simpler.Tests.Core.Mocks
{
    public class MockJob : Job
    {
        public override void Run()
        {
            throw new NotImplementedException();
        }
    }
}