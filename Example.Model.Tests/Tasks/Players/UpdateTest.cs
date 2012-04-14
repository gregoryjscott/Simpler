using Example.Model.Entities;
using Example.Model.Jobs.Players;
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
            var player = new Player
                         {
                             PlayerId = 1,
                             FirstName = "Something",
                             LastName = "Different",
                             TeamId = 2
                         };

            Test<Update>.Create()
                .Arrange(t => t.Set(new Update.In {Player = player}))
                .Act()
                .Assert(t =>
                            {
                                var updatedPlayer = Job.Create<FetchPlayer>()
                                    .Set(new FetchPlayer.In
                                             {
                                                 PlayerId = player.PlayerId.GetValueOrDefault()
                                             })
                                    .Get().Player;

                                Assert.That(updatedPlayer.LastName, Is.EqualTo("Different"));
                            });
        }
    }
}
