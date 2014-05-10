using Simpler;
using System;

public class OutputStats : OutTask<OutputStats.Output>
{
    public class Output
    {
        public int ReturnValue { get; set; }
    }

    public CheckTables CheckTables { get; set; }
    public FetchTeams FetchTeams { get; set; }
    public FetchManager FetchManager { get; set; }

    public override void Execute()
    {
        try
        {
            CheckTables.In.TableNames = new string[] {"Teams", "Managers"};
            CheckTables.Execute();
        }
        catch
        {
            Out.ReturnValue = 50;
            return;
        }

        FetchTeams.Execute();
        Console.WriteLine("Baseball has {0} teams.", FetchTeams.Out.Teams.Length);

        FetchManager.In.TeamName = "Cardinals";
        FetchManager.Execute();
        Console.WriteLine("Cardinals skipper is {0}", FetchManager.Out.Manager.Name);
    }
}
    