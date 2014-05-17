using System;
using System.Collections.Generic;
using System.Linq;
using Simpler;
using Examples.Models;

namespace Examples.Tasks
{
    [Log]
    public class OutputBestTeams: Task
    {
        public FetchTeams FetchTeams { get; set; }
        public FindBestTeam FindBestTeam { get; set; }
        public OutputStat OutputStat { get; set; }

        public override void Execute()
        {
            FetchTeams.Execute();
            var allTeams = FetchTeams.Out.Teams;

            var divisions = new[] {
                Filter(allTeams, "American", "East"),
                Filter(allTeams, "American", "Central"),
                Filter(allTeams, "American", "West"),
                Filter(allTeams, "National", "East"),
                Filter(allTeams, "National", "Central"),
                Filter(allTeams, "National", "West")
            };

            foreach (var division in divisions)
            {
                FindBestTeam.In.Teams = division.Teams;
                FindBestTeam.Execute();
                var team = FindBestTeam.Out.BestTeam;
                var percent = FindBestTeam.Out.WorldSeriesPercent;

                OutputStat.In.Stat = new Stat {
                    Question = String.Format(Question, division.Name),
                    Answer = team.Name,
                    Details = String.Format(Details, team.WorldSeries, percent, team.Age),
                };
                OutputStat.Execute();
            }
        }

        const string Question = "Who is {0}'s best team?";
        const string Details = "They've won the World Series {0} times ({1}%) in their {2} years.";

        #region Helpers
        static dynamic Filter(IEnumerable<Team> allTeams, string league, string division)
        {
            return new {
                Name = String.Format("{0} League {1}", league, division),
                Teams = allTeams.Where(t => t.League == league && t.Division == division).ToArray()
            };
        }
        #endregion
    }
}
