using Simpler.Testing.Tasks;

namespace Simpler.Testing
{
    // the nUnit test that looks for all task tests
    public class TaskTests
    {
        public void run_all_task_tests()
        {
            var runAllTaskTests = TaskFactory<RunAllTaskTests>.Create();
            runAllTaskTests.Execute();
        }
    }
}
