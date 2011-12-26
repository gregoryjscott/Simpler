using System;

namespace Simpler.Tests.Mocks
{
    public class MockTitoTaskUsingPrimitives : TitoTask<int, string>
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
