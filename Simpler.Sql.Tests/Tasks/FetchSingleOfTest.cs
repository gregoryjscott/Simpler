using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Simpler.Sql.Tasks;
using Simpler.Sql.Tests.Mocks;
using System.Data;
using Moq;

namespace Simpler.Sql.Tests.Tasks
{
    [TestFixture]
    public class FetchSingleOfTest
    {
        [Test]
        public void should_return_one_object_using_the_record_returned_by_the_select_command()
        {
            // Arrange
            var task = new FetchSingleOf<MockObject>();

            var table = new DataTable();
            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Age", Type.GetType("System.Int32"));
            table.Rows.Add(new object[] { "John Doe", "21" });

            var mockSelectCommand = new Mock<IDbCommand>();
            mockSelectCommand.Setup(command => command.ExecuteReader()).Returns(table.CreateDataReader());
            task.SelectCommand = mockSelectCommand.Object;

            // Act
            task.Execute();

            // Assert
            Assert.That(task.ObjectFetched.Name, Is.EqualTo("John Doe"));
        }
    }
}
