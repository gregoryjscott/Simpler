using System;
using NUnit.Framework;
using Saber.Tasks.Players;
using Simpler;

namespace Saber.Tests.Tasks.Players
{
    [TestFixture]
    public class ShowTest
    {
        [SetUp]
        public void SetDataDirectoryForConnectionString()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory + @"\App_Data");
        }

        [Test]
        public void should_return_a_player()
        {
            Test<Show>.New()
                .Arrange(t => t.Input = new Show.In {PlayerId = 1} )
                .Act()
                .Assert(t => Assert.That(t.Output.Player.PlayerId, Is.GreaterThan(0)));
        }
    }
}
