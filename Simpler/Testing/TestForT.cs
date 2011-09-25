namespace Simpler.Testing
{
    public delegate void SetupFor<T>(T task);
    public delegate void VerificationFor<T>(T task);

    public class TestFor<T> : Test where T : Task
    {
        public SetupFor<T> Setup { get; set; }
        public VerificationFor<T> Verify { get; set; }
    }
}
