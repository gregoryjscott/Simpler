using System.Diagnostics;
using NUnit.Framework;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests
{
    [TestFixture]
    public class ParallelTest
    {
        [Test]
        public void can_run_tasks_faster_in_parallel()
        {
            // Arrange
            Task[] tasks = new[] { Task.New<MockSlowTask>(), Task.New<MockSlowTask>() };

            // Act
            var stopwatch = Stopwatch.StartNew();
            foreach (var task in tasks)
            {
                task.Execute();
            }
            var sequential = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            Parallel.Execute(tasks);
            var parallel = stopwatch.ElapsedMilliseconds;

            // Assert
            Assert.That(parallel, Is.LessThan(sequential));
        }
    }
}
