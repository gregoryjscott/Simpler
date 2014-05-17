using NUnit.Framework;
using Simpler;

namespace Baseball.Tasks
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
