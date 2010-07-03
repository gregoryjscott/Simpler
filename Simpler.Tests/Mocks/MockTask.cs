using System;

namespace Simpler.Tests.Mocks
{
    public class MockTask : Task
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}