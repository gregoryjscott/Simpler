using Simpler.Injection;

namespace Simpler.Tests.Mocks
{
    [InjectSubTasks]
    public class MockDynamicTask : DynamicTask
    {
        public new dynamic In { get { return base.In; } set { base.In = value; } }
        public new dynamic Out { get { return base.Out; } set { base.Out = value; } }

        public MockDynamicSubTask MockDynamicSubTask { get; set; }

        public override void Execute()
        {
            MockDynamicSubTask.Execute();
            Out = new
                          {
                              InputsReceived = In,
                              SubTaskOutputs = MockDynamicSubTask.Out
                          };
        }
    }
}
