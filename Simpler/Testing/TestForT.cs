namespace Simpler.Testing
{
    public delegate void RunFor<T>(T task);

    public class TestFor<T> : Test where T : Task
    {
        public new RunFor<T> Run { get; set; }
    }
}
