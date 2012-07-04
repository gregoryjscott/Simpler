using NUnit.Framework;

namespace Simpler
{
    [TestFixture]
    public class AssmemblyTest
    {
        [Test]
        public void Assembly() { Describe.Assembly("Simpler"); }
    }
}

namespace Simpler.Data.Jobs
{
    [TestFixture]
    public class JobTests
    {
        [Test]
        public void BuildObject() { Describe.Job<BuildObject<object>>(); }

        [Test]
        public void BuildParameters() { Describe.Job<BuildParameters>(); }

        [Test]
        public void ExecuteAction() { Describe.Job<ExecuteAction>(); }

        [Test]
        public void FetchMany() { Describe.Job<FetchMany<object>>(); }

        [Test]
        public void FindParameters() { Describe.Job<FindParameters>(); }
    }
}
