using System;

namespace Simpler.Mocks
{
    public class MockTask : Task
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}