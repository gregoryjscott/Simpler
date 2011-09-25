namespace Simpler.Testing
{
    public delegate Task Setup();
    public delegate void Verification(Task task);

    public class Test
    {
        public string Expectation { get; set; }
        public Setup Setup { get; set; }
        public Verification Verify { get; set; }
    }
}