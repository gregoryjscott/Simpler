using System;

namespace Simpler.Tests.Core.Mocks
{
    public class MockSubTask<T> : Task, IDisposable
    {
        public bool DisposeWasCalled { get; set; }
        public bool WasExecuted { get; set; }

        public override void Execute()
        {
            WasExecuted = true;
        }

        public void Dispose()
        {
            DisposeWasCalled = true;
        }
    }
}
