using Examples.Tasks;
using Simpler;

public class Program
{
    static int Main(string[] args)
    {
        var outputBestTeams = Task.New<OutputBestTeams>();
        outputBestTeams.Execute();
        return 0;
    }
}