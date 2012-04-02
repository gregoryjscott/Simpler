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

            Test<Update>.New()
                .Arrange(t => t.Input = new Update.In {Player = player})
                .Act()
                .Assert(t =>
                            {
                                var updatedPlayer = Invoke<FetchPlayer>.New()
                                    .Set(t2 => t2.Input = new FetchPlayer.In
                                                              {
                                                                  PlayerId = player.PlayerId.GetValueOrDefault()
                                                              })
                                    .Get().Output.PlayerData;

                                Assert.That(updatedPlayer.LastName, Is.EqualTo("Different"));
                            });
        }
    }
}
