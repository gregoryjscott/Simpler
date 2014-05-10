using Simpler;
using System;

public class GoodProgram
{
    static int Main(string[] args)
    {
        try
        {
            var checkTables = Task.New<CheckTables>();
            checkTables.In.TableNames = new string[] {"Teams", "Managers"};
            checkTables.Execute();
        }
        catch { return 50; }

        var fetchTeams = Task.New<FetchTeams>();
        fetchTeams.Execute();
        Console.WriteLine("Baseball has {0} teams.", fetchTeams.Out.Teams.Length);

        var fetchManager = Task.New<FetchManager>();
        fetchManager.In.TeamName = "Cardinals";
        fetchManager.Execute();
        Console.WriteLine("Cardinals skipper is {0}", fetchManager.Out.Manager.Name);

        return 0;
    }
}
