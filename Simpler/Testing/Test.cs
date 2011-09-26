namespace Simpler.Testing
{
    public delegate void Run(dynamic task);

    public class Test
    {
        public string Expectation { get; set; }
        public Run Run { get; set; }
    }
}