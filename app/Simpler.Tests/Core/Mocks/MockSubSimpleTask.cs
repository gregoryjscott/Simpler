using System;

namespace Simpler.Tests.Core.Mocks
{
	public class MockSubSimpleTask<T> : SimpleTask, IDisposable
	{
        public override void Execute()
        {
            throw new NotImplementedException();
        }

        public bool DisposeWasCalled { get; set; }

        public void Dispose()
        {
            DisposeWasCalled = true;
        }
    }
}
