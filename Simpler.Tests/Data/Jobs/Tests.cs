using NUnit.Framework;

namespace Simpler.Data.Jobs
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Simpler() { Test.Assembly("Simpler"); }

        [Test]
        public void BuildObject() { Test.Job<BuildObject<object>>(); }
    }
}
