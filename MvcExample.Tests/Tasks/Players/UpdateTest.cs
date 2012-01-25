using System;
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
            var data = new Resources.PlayersResource.Data
                         {
                             PlayerId = 1,
                             FirstName = "Something",
                             LastName = "Different",
                             TeamId = 2
                         };

            Task.Create<Update>()
                .SetInputs(new {Data = data})
                .Execute();

            var outputs = Task.Create<Show>()
                .SetInputs(new { PlayerId = 1})
                .GetOutputs();

            Assert.That(outputs.Data.LastName, Is.EqualTo("Different"));
        }
    }
}
