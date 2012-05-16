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
                .Arrange(job => job.Set(new UpdatePlayer.Input {Player = player}))
                .Act()
                .Assert(
                    job =>
                    {
                        var updatedPlayer = Job.New<FetchPlayer>()
                            .Set(new FetchPlayer.Input
                                 {
                                     PlayerId = player.PlayerId.GetValueOrDefault()
                                 })
                            .Get().Player;

                        Assert.That(updatedPlayer.LastName, Is.EqualTo("Different"));
                    });
        }
    }
}
