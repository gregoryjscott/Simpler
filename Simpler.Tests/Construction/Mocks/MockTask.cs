using System;

namespace Simpler.Tests.Construction.Mocks
{
    public class MockTask : Task
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}