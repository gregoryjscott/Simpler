using System;

namespace Simpler.Tests.Core.Mocks
{
    public class MockTask : Task
    {
        public override void Run()
        {
            throw new NotImplementedException();
        }
    }
}