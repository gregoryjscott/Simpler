using Baseball.Tasks;
using Simpler;

namespace Baseball
{
    public class Program
    {
        static int Main(string[] args)
        {
            var outputBestTeams = Task.New<OutputBestTeams>();
            outputBestTeams.Execute();
            return 0;
        }
    }
}