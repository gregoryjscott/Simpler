using System.Reflection;
using NUnit.Framework;
using Simpler.Testing.Tasks;

namespace Simpler.Testing
{
    [TestFixture]
    public class TaskTests
    {
        [Test]
        public void run_all_task_tests_in_executing_assembly()
        {
            var runAllTaskTests = TaskFactory<RunTaskTestsInAssembly>.Create();
            runAllTaskTests.AssemblyWithTasks = Assembly.GetExecutingAssembly();
            runAllTaskTests.Execute();
        }
    }
}
