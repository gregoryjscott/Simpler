using System;
using Simpler.Testing.Tasks;

namespace Simpler.Testing
{
    public static class RunTests
    {
        public static void All()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var runAllTaskTests = TaskFactory<RunTaskTestsInAssembly>.Create();
            foreach (var assembly in assemblies)
            {
                runAllTaskTests.AssemblyWithTasks = assembly;
                runAllTaskTests.Execute();
            }
        }
    }
}
