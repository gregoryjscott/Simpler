using System;
using System.Linq;
using Simpler;
using Baseball.Models;

namespace Baseball.Tasks
{
    public class FindBestTeam: InOutTask<FindBestTeam.Input, FindBestTeam.Output>
    {
        public class Input
        {
            public Team[] Teams { get; set; }
        }

        public class Output
        {
            public Team BestTeam { get; set; }
            public double WorldSeriesPercent { get; set; }
        }

        public override void Execute()
        {
            var wonWorldSeries = In.Teams.Select(t => new {
                Team = t,
                Percent = t.WorldSeries != 0
                    ? Math.Round(t.WorldSeries / (double)(t.Age) * 100, 2)
                    : 0
            });
            var best = wonWorldSeries.OrderByDescending(t => t.Percent).First();

            Out.BestTeam = best.Team;
            Out.WorldSeriesPercent = best.Percent;
        }
    }
}
