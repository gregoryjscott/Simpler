using NUnit.Framework;
using Simpler.Data.Tasks;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Assembly() { Describe.Assembly("Simpler"); }

        [Test]
        public void RunClassTest()
        {
            var name = Run<MockInOutTask>
                .Set(In => In.Name = "something")
                .Get().MockObject.Name;

            Check.That(name == "something", "Expected name to be 'something'.");
        }
    }

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