using System;
using NUnit.Framework;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests
{
    [TestFixture]
    public class ExecuteTest
    {
        [Test]
        public void should_call_setup_action_if_provided()
        {
            var setupWasCalled = false;
            Action<MockTask> setup = t => {
                setupWasCalled = true;
            };
            Execute.Now(setup);

            Assert.That(setupWasCalled, Is.True);
        }

        [Test]
        public void should_block_when_executed_now()
        {
            Assert.Throws<MockException>(() => Execute.Now<MockTaskThatThrowsWithAttributes>());
        }
    }
}
