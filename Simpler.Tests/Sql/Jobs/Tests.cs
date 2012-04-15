using NUnit.Framework;
using Simpler.Sql.Jobs;
using Simpler.Tests.Mocks;

namespace Simpler.Tests.Sql.Jobs
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Simpler() { Test<Job>.InAssembly("Simpler"); }

        [Test]
        public void _Build() { Job.New<_Build<MockObject>>().Test(); }
    }
}
