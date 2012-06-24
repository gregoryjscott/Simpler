using NUnit.Framework;
using Simpler.Data.Jobs;
using Simpler.Tests.Mocks;

namespace Simpler.Tests.Data.Jobs
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Everything() { Test.Everything(); }

        [Test]
        public void Simpler() { Test.Assembly("Simpler"); }

        [Test]
        public void _Build() { Job.New<BuildObject<MockObject>>().Specs(); }
    }
}
