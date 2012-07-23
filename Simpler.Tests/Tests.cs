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