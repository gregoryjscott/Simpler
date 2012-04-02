using System;
using MvcExample.Tasks.Players;
using NUnit.Framework;
using Simpler;

namespace MvcExample.Tests.Tasks.Players
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
            Test<Show>.Create()
                .Arrange(t => t.Input = new Show.In {PlayerId = 1})
                .Act()
                .Assert(t => Assert.That(t.Output.Player.PlayerId, Is.GreaterThan(0)));
        }
    }
}
