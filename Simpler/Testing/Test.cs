namespace Simpler.Testing
{
    public delegate void Setup(dynamic task);
    public delegate void Verification(dynamic task);

    public class Test
    {
        public Test()
        {
            // This allows Setup to be optional when creating a test using object initialization.
            Setup = (task) => { };
        }

        public string Expectation { get; set; }
        public Setup Setup { get; set; }
        public Verification Verify { get; set; }
    }
}