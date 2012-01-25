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
            var outputs = Task.Create<Show>()
                .SetInputs(new { PlayerId = 1})
                .GetOutputs();

            Assert.That(outputs.Player.PlayerId, Is.GreaterThan(0));
        }
    }
}
