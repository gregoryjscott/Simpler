using Simpler;
using System;

// 4. Implement the business logic.

public class PredictWorldSeries : InOutTask<PredictWorldSeries.Input, PredictWorldSeries.Output>
{
    public class Input
    {
        public int Year { get; set; }
    }

    public class Output
    {
        public Team WinningTeam { get; set; }
    }

    public override void Execute()
    {
        // Be lazy and just include the National League Central teams.
        var teams = new[] {
            new Team { Name = "Cardinals" },
            new Team { Name = "Pirates" },
            new Team { Name = "Brewers" },
            new Team { Name = "Reds" },
            new Team { Name = "Cubs" }
        };

        // You can't predict baseball! Just pick a random team.
        Out.WinningTeam = PickRandomTeam(teams);

        // But be realistic...
        while (Out.WinningTeam.Name == "Cubs")
        {
            Out.WinningTeam = PickRandomTeam(teams);
        }
    }

    #region Helpers

    Team PickRandomTeam(Team[] teams)
    {
        var random = new Random();
        return teams[random.Next(teams.Length)];
    }

    #endregion
}

// 5. Verify the test passes.

//```
//    .......................................................
//    Tests run: 58, Errors: 0, Failures: 0, Inconclusive: 0, Time: 1.3800311 seconds
//    Not run: 0, Invalid: 0, Ignored: 0, Skipped: 0
//
//    ```
