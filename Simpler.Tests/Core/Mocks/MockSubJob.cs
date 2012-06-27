using System;

namespace Simpler.Tests.Core.Mocks
{
	public class MockSubJob<T> : Job, IDisposable
	{
        public override void Run()
        {
            throw new System.NotImplementedException();
        }

        public bool DisposeWasCalled { get; set; }

        public void Dispose()
        {
            DisposeWasCalled = true;
        }
    }
}
