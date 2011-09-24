using NUnit.Framework;
using Simpler.Testing.Tasks;

namespace Simpler.Testing
{
    [TestFixture]
    public class TaskTests
    {
        [Test]
        public void run_all_task_tests()
        {
            var runAllTaskTests = TaskFactory<RunAllTaskTests>.Create();
            runAllTaskTests.Execute();
        }
    }
}
