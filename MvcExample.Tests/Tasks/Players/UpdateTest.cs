using System;
using MvcExample.Models.Players;
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
            // Arrange
            var update = TaskFactory<Update>.Create();
            update.Inputs = new PlayerEdit {PlayerId = 1, FirstName = "Something", LastName = "Different", TeamId = 2};

            // Act
            update.Execute();

            // Assert
            var show = TaskFactory<Show>.Create();
            show.Inputs = new PlayerKey(1);
            show.Execute();
            Assert.That(show.Outputs.Model.Name, Is.EqualTo("Something Different"));
        }
    }
}
