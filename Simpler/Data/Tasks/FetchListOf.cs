using System;
using System.Collections.Generic;
using System.Data;
using Moq;
using NUnit.Framework;
using Simpler.Testing;
using Simpler.Tests.Mocks;

namespace Simpler.Data.Tasks
{
    /// <summary>
    /// Task that will create a list of objects of the given type T using the results of the given command.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the list to return.</typeparam>
    public class FetchListOf<T> : Task
    {
        // Inputs
        public virtual IDbCommand SelectCommand { get; set; }

        // Outputs
        public virtual T[] ObjectsFetched { get; private set; }

        // Sub-tasks
        public virtual UseDataRecordToBuild<T> UseDataRecordToBuild { get; set; }

        public override void Execute()
        {
            // Create the sub-tasks.
            if (UseDataRecordToBuild == null) UseDataRecordToBuild = new UseDataRecordToBuild<T>();

            var objectList = new List<T>();

            using (var dataReader = SelectCommand.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    UseDataRecordToBuild.DataRecord = dataReader;
                    UseDataRecordToBuild.Execute();
                    objectList.Add(UseDataRecordToBuild.Object);
                }
            }

            ObjectsFetched = objectList.ToArray();
        }

        public override Test[] Tests()
        {
            return
                new[]
                {
                    new TestFor<FetchListOf<MockObject>>
                    {
                        Expectation = "return an object for each record returned by the select command",

                        Run = (task) =>
                              {
                                  // Arrange
                                  var testData = new DataTable();
                                  testData.Columns.Add("Name", Type.GetType("System.String"));
                                  testData.Columns.Add("Age", Type.GetType("System.Int32"));
                                  testData.Rows.Add(new object[] {"John Doe", "21"});
                                  testData.Rows.Add(new object[] {"Jane Doe", "19"});

                                  var mockSelectCommand = new Mock<IDbCommand>();
                                  mockSelectCommand.Setup(command => command.ExecuteReader())
                                      .Returns(testData.CreateDataReader());
                                  task.SelectCommand = mockSelectCommand.Object;

                                  // Act
                                  task.Execute();

                                  // Assert
                                  Assert.That(task.ObjectsFetched.Length, Is.EqualTo(2));
                                  Assert.That(task.ObjectsFetched[0].Name, Is.EqualTo("John Doe"));
                                  Assert.That(task.ObjectsFetched[1].Name, Is.EqualTo("Jane Doe"));
                              }
                    },
                    //new Test
                    //{
                    //    Expectation = "do the same thing using the Task that sends a dynamic task",

                    //    Run = (task) =>
                    //          {
                    //              // Arrange
                    //              var testData = new DataTable();
                    //              testData.Columns.Add("Name", Type.GetType("System.String"));
                    //              testData.Columns.Add("Age", Type.GetType("System.Int32"));
                    //              testData.Rows.Add(new object[] {"John Doe", "21"});
                    //              testData.Rows.Add(new object[] {"Jane Doe", "19"});

                    //              var mockSelectCommand = new Mock<IDbCommand>();
                    //              mockSelectCommand.Setup(command => command.ExecuteReader())
                    //                  .Returns(testData.CreateDataReader());
                    //              task.SelectCommand = mockSelectCommand.Object;

                    //              // Act
                    //              task.Execute();

                    //              // Assert
                    //              Assert.That(task.ObjectsFetched.Length, Is.EqualTo(2));
                    //              Assert.That(task.ObjectsFetched[0].Name, Is.EqualTo("John Doe"));
                    //              Assert.That(task.ObjectsFetched[1].Name, Is.EqualTo("Jane Doe"));
                    //          }
                    //}
                };
        }
    }
}
