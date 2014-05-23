using Centroid;
using Simpler;
using System;
using System.Collections.Generic;
using System.Globalization;
using Baseball.Models;

namespace Baseball.Tasks
{
    public class FetchTeams: OutTask<FetchTeams.Output>
    {
        public class Output
        {
            public Team[] Teams { get; set; }
        }

        public override void Execute()
        {
            var teams = new List<Team>();

            dynamic baseball = Config.FromFile("examples.json");
            foreach (dynamic team in baseball.Teams)
            {
                teams.Add(new Team {
                    League = team.league,
                    Division = team.division,
                    Name = team.name,
                    FirstSeason = team.since,
                    G = Int64.Parse((string)team.g, NumberStyles.AllowThousands),
                    W = Int64.Parse((string)team.w, NumberStyles.AllowThousands),
                    Pennants = team.pennants,
                    WorldSeries = team.worldSeries,
                    Playoffs = team.playoffs,
                    R = Int64.Parse((string)team.r, NumberStyles.AllowThousands),
                    AB = Int64.Parse((string)team.ab, NumberStyles.AllowThousands),
                    H = Int64.Parse((string)team.h, NumberStyles.AllowThousands),
                    HR = Int64.Parse((string)team.hr, NumberStyles.AllowThousands),
                    BA = team.ba,
                    RA = Int64.Parse((string)team.ra, NumberStyles.AllowThousands),
                    Era = team.era
                });
            }

            Out.Teams = teams.ToArray();
        }
    }
}
