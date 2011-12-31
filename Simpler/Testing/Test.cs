namespace Simpler.Testing
{
    public delegate void Run(dynamic task);

    public abstract class Test
    {
        public string Expectation { get; set; }
        public Run Run { get; set; }
    }
}