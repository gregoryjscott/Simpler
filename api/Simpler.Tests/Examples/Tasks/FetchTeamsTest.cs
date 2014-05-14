using Examples.Tasks;
using NUnit.Framework;
using Simpler;

namespace Tests.Examples.Tasks
{
    [TestFixture]
    public class FetchTeamsTest
    {
        [Test]
        public void returns_30_teams()
        {
            var fetchTeams = Task.New<FetchTeams>();
            fetchTeams.Execute();

            Assert.That(fetchTeams.Out.Teams.Length, Is.EqualTo(30));
        }
    }
}
