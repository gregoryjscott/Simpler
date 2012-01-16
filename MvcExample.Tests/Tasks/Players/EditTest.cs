using System;
using MvcExample.Models.Players;
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
            var edit = TaskFactory<Edit>.Create();
            edit.Inputs = new PlayerKey(1);
            edit.Execute();
            Assert.That(edit.Outputs.Model.PlayerId, Is.EqualTo(1));
        }
    }
}
