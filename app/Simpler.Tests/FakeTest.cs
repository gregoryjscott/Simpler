using NUnit.Framework;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests
{
    [TestFixture]
    public class FakeTest
    {
        [Test]
        public void should_create_fake_task()
        {
            var mockTask = Fake.Task<MockTask>();

            Check.That(mockTask != null, "MockTask was not created.");
        }

        [Test]
        public void should_override_execute_with_given_action()
        {
            var overrideHappened = false;
            var mockTask = Fake.Task<MockTask>(t => overrideHappened = true);

            mockTask.Execute();

            Check.That(overrideHappened, "Execute override did not work.");
        }
    }
}
