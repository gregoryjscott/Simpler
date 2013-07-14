using System;
using System.Linq;
using NUnit.Framework;
using Simpler.Data.Tasks;
using Moq;
using System.Data;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class FetchManyTest
    {
        [Test]
        public void should_return_an_object_for_each_record_returned_by_the_select_command()
        {
            // Arrange
            var task = SimpleTask.New<FetchMany<MockObject>>();

            var table = new DataTable();
            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Age", Type.GetType("System.Int32"));
            table.Rows.Add(new object[] { "John Doe", "21" });
            table.Rows.Add(new object[] { "Jane Doe", "19" });

            var mockSelectCommand = new Mock<IDbCommand>();
            mockSelectCommand.Setup(command => command.ExecuteReader()).Returns(table.CreateDataReader());
            task.In.SelectCommand = mockSelectCommand.Object;

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.ObjectsFetched.Count(), Is.EqualTo(2));
            Assert.That(task.Out.ObjectsFetched[0].Name, Is.EqualTo("John Doe"));
            Assert.That(task.Out.ObjectsFetched[1].Name, Is.EqualTo("Jane Doe"));
        }
    }
}