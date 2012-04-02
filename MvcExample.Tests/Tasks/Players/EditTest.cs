using System;
using MvcExample.Tasks.Players;
using NUnit.Framework;
using Simpler;

namespace MvcExample.Tests.Tasks.Players
{
    [TestFixture]
    public class EditTest
    {
        [SetUp]
        public void SetDataDirectoryForConnectionString()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory + @"\App_Data");
        }

        [Test]
        public void should_return_a_player_for_editing()
        {
            Test<Edit>.Create()
                .Arrange(t =>
                             {
                                 t.Input = new Edit.In {PlayerId = 1};
                             })
                .Act()
                .Assert(t =>
                            {
                                var player = t.Output.Player;
                                Assert.That(player.PlayerId, Is.EqualTo(1));
                            });
        }
    }
}
