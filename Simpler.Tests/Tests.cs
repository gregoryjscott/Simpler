using NUnit.Framework;
using Simpler.Data.Tasks;

namespace Simpler.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Assembly() { Describe.Assembly("Simpler"); }
    }

    [TestFixture]
    public class TaskTests
    {
        [Test]
        public void BuildObject() { Describe.Task<BuildObject<object>>(); }

        [Test]
        public void BuildParameters() { Describe.Task<BuildParameters>(); }

        [Test]
        public void ExecuteAction() { Describe.Task<ExecuteAction>(); }

        [Test]
        public void FetchMany() { Describe.Task<FetchMany<object>>(); }

        [Test]
        public void FindParameters() { Describe.Task<FindParameters>(); }
    }
}