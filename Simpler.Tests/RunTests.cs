using NUnit.Framework;
using Simpler.Tests.Core.Mocks;

namespace Simpler
{
    [TestFixture]
    public class RunTests
    {
        [Test]
        public void it_works()
        {
            var name = Run<MockInOutJob>
                .Set(In => In.Name = "something")
                .Get().MockObject.Name;

            Check.That(name == "something", "Expected name to be 'something'.");
        }
    }
}
