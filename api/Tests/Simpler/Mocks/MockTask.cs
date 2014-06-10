using System;

namespace Simpler.Mocks
{
    public class MockTask : Task
    {
        public bool WasExecuted { get; set; }

        public override void Execute()
        {
            WasExecuted = true;
        }
    }
}