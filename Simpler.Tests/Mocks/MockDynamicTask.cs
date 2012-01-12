using System;
using Simpler.Injection;

namespace Simpler.Tests.Mocks
{
    [InjectSubTasks]
    public class MockDynamicTask : DynamicTask
    {
        public MockDynamicSubTask MockDynamicSubTask { get; set; }

        public override void Execute()
        {
            MockDynamicSubTask.Execute();
            Outputs = new
                          {
                              InputsReceived = Inputs,
                              SubTaskOutputs = MockDynamicSubTask.Outputs
                          };
        }
    }
}
