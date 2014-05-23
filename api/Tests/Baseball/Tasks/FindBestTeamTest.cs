using NUnit.Framework;
using Simpler;
using Baseball.Models;

namespace Baseball.Tasks
{
    [TestFixture]
    public class FindBestTeamTest
    {
        [Test]
        public void picks_team_with_highest_World_Series_percentage()
        {
            var findBestTeam = Task.New<FindBestTeam>();
            findBestTeam.In.Teams = new[] {
                new Team {
                    Name = "Good Team",
                    FirstSeason = 2010,
                    WorldSeries = 1
                },
                new Team {
                    Name = "Better Team",
                    FirstSeason = 2010,
                    WorldSeries = 3
                }
            };
            findBestTeam.Execute();

            Assert.That(findBestTeam.Out.BestTeam.Name, Is.EqualTo("Better Team"));
            Assert.That(findBestTeam.Out.WorldSeriesPercent, Is.EqualTo(75));
        }
    }
}
