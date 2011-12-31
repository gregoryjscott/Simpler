using NUnit.Framework;
using Simpler.Testing;

namespace Simpler.Tests
{
    [TestFixture]
    public class SimplerTests
    {
        [Test]
        public void run_all_task_tests_in_Simpler()
        {
            RunTests.All();
        }
    }
}
