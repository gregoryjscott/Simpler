using Example.Model.Jobs.Players;
using NUnit.Framework;
using Simpler;

namespace Example.Model.Tests.Jobs.Players
{
    [TestFixture]
    public class IndexTest
    {
        [SetUp]
        public void SetUp()
        {
            Config.SetDataDirectory();
        }

        [Test]
        public void should_return_list_of_players()
        {
            Test<FetchPlayers>.New()
                .Act()
                .Assert(job => Assert.That(job._Out.Players.Length, Is.GreaterThan(0)));
        }
    }
}
