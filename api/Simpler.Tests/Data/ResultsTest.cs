using System;
using System.Data;
using Moq;
using NUnit.Framework;
using Simpler.Data;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Data
{
    [TestFixture]
    public class ResultsTest
    {
        static DataTable SetupPersonTable()
        {
            var table = new DataTable();
            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Age", Type.GetType("System.Int32"));
            table.Rows.Add(new object[] { "John Doe", "21" });
            table.Rows.Add(new object[] { "Jane Doe", "19" });
            return table;
        }

        [Test]
        public void should_read_result()
        {
            // Arrange
            var reader = SetupPersonTable().CreateDataReader();
            var results = new Results(reader);

            // Act
            var objects = results.Read<MockPerson>();

            // Assert
            Assert.That(objects.Length, Is.EqualTo(2));
            Assert.That(objects[0].Name, Is.EqualTo("John Doe"));
            Assert.That(objects[1].Name, Is.EqualTo("Jane Doe"));
        }

        [Test]
        public void should_advance_to_next_result_after_every_read()
        {
            // Arrange
            var mockReader = new Mock<IDataReader>();
            mockReader.Setup(reader => reader.Read()).Returns(false);
            var results = new Results(mockReader.Object);

            results.Read<dynamic>();

            mockReader.Verify(reader => reader.NextResult(), Times.Once());
        }

        [Test]
        public void should_throw_if_read_attempted_after_all_results_have_been_read()
        {
            // Arrange
            var reader = SetupPersonTable().CreateDataReader();
            var results = new Results(reader);
            results.Read<MockPerson>();

            // Act & Assert
            Assert.Throws<CheckException>(() => results.Read<MockPerson>());
        }

        [Test]
        public void should_automatically_dispose_reader_after_all_results_have_been_read()
        {
            // Arrange
            var mockReader = new Mock<IDataReader>();
            mockReader.Setup(reader => reader.FieldCount).Returns(2);
            mockReader.Setup(reader => reader.GetName(0)).Returns("Name");
            mockReader.Setup(reader => reader["Name"]).Returns("John Doe");
            mockReader.Setup(reader => reader.GetName(1)).Returns("Age");
            mockReader.Setup(reader => reader["Age"]).Returns(21);
            var results = new Results(mockReader.Object);

            // Act
            results.Read<MockPerson>();

            // Assert
            mockReader.Verify(mdr => mdr.Dispose(), Times.Once());
        }
    }
}