using System;
using MvcExample.Resources;
using MvcExample.Tasks.Players;
using NUnit.Framework;
using Simpler;

namespace MvcExample.Tests.Tasks.Players
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

            Task.Create<Update>()
                .SetInputs(new {Player = player})
                .Execute();

            var outputs = Task.Create<Show>()
                .SetInputs(new { PlayerId = 1})
                .GetOutputs();

            Assert.That(outputs.Player.LastName, Is.EqualTo("Different"));
        }
    }
}
