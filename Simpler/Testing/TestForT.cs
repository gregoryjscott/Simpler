namespace Simpler.Testing
{
    public delegate void SetupFor<T>(T task);
    public delegate void VerificationFor<T>(T task);

    public class TestFor<T> : Test where T : Task
    {
        public TestFor()
        {
            // This allows Setup to be optional when creating a test using object initialization.
            Setup = (task) => { };
        }

        public SetupFor<T> Setup { get; set; }
        public VerificationFor<T> Verify { get; set; }
    }
}
