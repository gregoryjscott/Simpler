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
        [Test]
        public void should_read_result()
        {
            var reader = SetupJohnAndJane();
            var results = new Results(reader);

            var objects = results.Read<MockPerson>();

            Assert.That(objects.Length, Is.EqualTo(2));
            Assert.That(objects[0].Name, Is.EqualTo("John Doe"));
            Assert.That(objects[1].Name, Is.EqualTo("Jane Doe"));
        }

        [Test]
        public void should_advance_to_next_result_after_every_read()
        {
            var mockReader = new Mock<IDataReader>();
            mockReader.Setup(reader => reader.Read()).Returns(false);
            var results = new Results(mockReader.Object);

            results.Read<dynamic>();

            mockReader.Verify(reader => reader.NextResult(), Times.Once());
        }

        [Test]
        public void should_throw_if_read_attempted_after_all_results_have_been_read()
        {
            var reader = SetupJohnAndJane();
            var results = new Results(reader);
            results.Read<MockPerson>();

            Assert.Throws<CheckException>(() => results.Read<MockPerson>());
        }

        [Test]
        public void should_automatically_dispose_reader_after_all_results_have_been_read()
        {
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.FieldCount).Returns(2);
            reader.Setup(r => r.GetName(0)).Returns("Name");
            reader.Setup(r => r["Name"]).Returns("John Doe");
            reader.Setup(r => r.GetName(1)).Returns("Age");
            reader.Setup(r => r["Age"]).Returns(21);
            var results = new Results(reader.Object);

            results.Read<MockPerson>();

            reader.Verify(mdr => mdr.Dispose(), Times.Once());
        }

        #region Helpers

        internal static IDataReader SetupJohnAndJane()
        {
            var table = new DataTable();
            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Age", Type.GetType("System.Int32"));
            table.Rows.Add(new object[] { "John Doe", "21" });
            table.Rows.Add(new object[] { "Jane Doe", "19" });
            return table.CreateDataReader();
        }

        internal static IDataRecord SetupJohn()
        {
            var dataRecord = new Mock<IDataRecord>();
            dataRecord.Setup(dr => dr.FieldCount).Returns(2);
            dataRecord.Setup(dr => dr.GetName(0)).Returns("Name");
            dataRecord.Setup(dr => dr["Name"]).Returns("John Doe");
            dataRecord.Setup(dr => dr.GetName(1)).Returns("Age");
            dataRecord.Setup(dr => dr["Age"]).Returns(21);
            return dataRecord.Object;
        }

        #endregion
    }
}