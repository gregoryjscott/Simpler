using System;
using Simpler.Injection;

namespace Simpler.Tests.Mocks
{
    [InjectSubTasks]
    public class MockTaskUsingDynamicProperties : Task
    {
        public MockSubTaskUsingDynamicProperties MockSubTaskUsingDynamicProperties { get; set; }

        public override void Execute()
        {
            MockSubTaskUsingDynamicProperties.Execute();
            Outputs = new { SubTaskOutputs = MockSubTaskUsingDynamicProperties.Outputs };
        }
    }
}
