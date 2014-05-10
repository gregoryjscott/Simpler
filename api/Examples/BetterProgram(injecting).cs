using Simpler;

public class BetterProgram
{
    static int Main(string[] args)
    {
        var outputStats = Task.New<OutputStats>();
        outputStats.Execute();
        return outputStats.Out.ReturnValue;
    }
}
