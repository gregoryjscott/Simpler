using System;
using NUnit.Framework;
using Saber.Models.Players;
using Saber.Tasks.Players;
using Simpler;

namespace Saber.Tests.Tasks.Players
{
    [TestFixture]
    public class UpdateTest
    {
        [SetUp]
        public void SetDataDirectoryForConnectionString()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory + @"\App_Data");
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
                .Arrange(t => t.Input = new Update.In {Player = player})
                .Act()
                .Assert(t =>
                            {
                                var updatedPlayer = Task.Create<FetchPlayer>()
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
