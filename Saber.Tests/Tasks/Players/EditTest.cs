using System;
using NUnit.Framework;
using Saber.Tasks.Players;
using Simpler;

namespace Saber.Tests.Tasks.Players
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
            var outputs = Task.Create<Edit>()
                .SetInputs(new {PlayerId = 1})
                .GetOutputs();

            Assert.That(outputs.Player.PlayerId, Is.EqualTo(1));
        }
    }
}
