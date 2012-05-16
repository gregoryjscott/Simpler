using Example.Model.Entities;
using Example.Model.Jobs;
using NUnit.Framework;
using Simpler;

namespace Example.Model.Tests.Jobs.Players
{
    [TestFixture]
    public class UpdateTest
    {
        [SetUp]
        public void SetUp()
        {
            Config.SetDataDirectory();
        }

        [Test]
        public void should_update_a_player()
        {
            var player =
                new Player
                {
                    PlayerId = 1,
                    FirstName = "Something",
                    LastName = "Different",
                    TeamId = 2
                };

            Test<UpdatePlayer>.New()
                .Arrange(job => job._In.Player = player)
                .Act()
                .Assert(
                    job =>
                        {
                            var fetch = Job.New<FetchPlayer>();
                            fetch._In.PlayerId = player.PlayerId.GetValueOrDefault();
                            fetch.Run();
                            var updatedPlayer = fetch._Out.Player;

                            Assert.That(updatedPlayer.LastName, Is.EqualTo("Different"));
                        });
        }
    }
}
