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
            var outputs = Task.Create<Edit>()
                .SetInputs(new {PlayerId = 1})
                .GetOutputs();

            Assert.That(outputs.Data.PlayerId, Is.EqualTo(1));
        }
    }
}
