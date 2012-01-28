namespace Simpler.Tests.Mocks
{
    public class MockDynamicSubTask : DynamicTask
    {
        public new dynamic In { get { return base.In; } set { base.In = value; } }
        public new dynamic Out { get { return base.Out; } set { base.Out = value; } }

        public override void Execute()
        {
            Out = new { SomeOutput = 9 };
        }
    }
}
