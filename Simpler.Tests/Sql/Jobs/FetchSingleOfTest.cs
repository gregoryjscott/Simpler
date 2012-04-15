//using System;
//using NUnit.Framework;
//using System.Data;
//using Moq;
//using Simpler.Sql.Jobs;
//using Simpler.Tests.Mocks;

//namespace Simpler.Tests.Data.Jobs
//{
//    [TestFixture]
//    public class FetchSingleOfTest
//    {
//        [Test]
//        public void should_return_one_object_using_the_record_returned_by_the_select_command()
//        {
//            // Arrange
//            var job = Job.New<FetchSingleOf<MockObject>>();

//            var table = new DataTable();
//            table.Columns.Add("Name", Type.GetType("System.String"));
//            table.Columns.Add("Age", Type.GetType("System.Int32"));
//            table.Rows.Add(new object[] { "John Doe", "21" });

//            var mockSelectCommand = new Mock<IDbCommand>();
//            mockSelectCommand.Setup(command => command.ExecuteReader()).Returns(table.CreateDataReader());
//            job.SelectCommand = mockSelectCommand.Object;

//            // Act
//            job.Run();

//            // Assert
//            Assert.That(job.ObjectFetched.Name, Is.EqualTo("John Doe"));
//        }

//        [Test]
//        public void should_throw_if_no_records_are_found()
//        {
//            var job = Job.New<FetchSingleOf<MockObject>>();

//            var table = new DataTable();

//            var mockSelectCommand = new Mock<IDbCommand>();
//            mockSelectCommand.Setup(command => command.ExecuteReader()).Returns(table.CreateDataReader());
//            job.SelectCommand = mockSelectCommand.Object;

//            // Act & Assert
//            Assert.Throws(typeof(InvalidOperationException), job.Run);
//        }

//        [Test]
//        public void should_throw_if_more_than_one_record_is_found()
//        {
//            var job = Job.New<FetchSingleOf<MockObject>>();

//            var table = new DataTable();
//            table.Columns.Add("Name", Type.GetType("System.String"));
//            table.Columns.Add("Age", Type.GetType("System.Int32"));
//            table.Rows.Add(new object[] { "John Doe", "21" });
//            table.Rows.Add(new object[] { "John Doe", "21" });

//            var mockSelectCommand = new Mock<IDbCommand>();
//            mockSelectCommand.Setup(command => command.ExecuteReader()).Returns(table.CreateDataReader());
//            job.SelectCommand = mockSelectCommand.Object;

//            // Act & Assert
//            Assert.Throws(typeof(InvalidOperationException), job.Run);
//        }
//    }
//}
