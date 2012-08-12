using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Moq;
using NUnit.Framework;
using Simpler.Core.Mocks;

namespace Simpler.Data.Tasks
{
    public class FetchMany<T> : InOutTask<FetchMany<T>.Input, FetchMany<T>.Output>  
    {
        public override void Specs()
        {
            It<FetchMany<MockObject>>.Should(
                "return an object for each record returned by the select command",
                it =>
                    {
                        var table = new DataTable();
                        table.Columns.Add("Name", Type.GetType("System.String"));
                        table.Columns.Add("Age", Type.GetType("System.Int32"));
                        table.Rows.Add(new object[] {"John Doe", "21"});
                        table.Rows.Add(new object[] {"Jane Doe", "19"});

                        var mockSelectCommand = new Mock<IDbCommand>();
                        mockSelectCommand.Setup(command => command.ExecuteReader()).Returns(table.CreateDataReader());
                        it.In.SelectCommand = mockSelectCommand.Object;

                        // Act
                        it.Execute();

                        // Assert
                        Assert.That(it.Out.ObjectsFetched.Count(), Is.EqualTo(2));
                        Assert.That(it.Out.ObjectsFetched[0].Name, Is.EqualTo("John Doe"));
                        Assert.That(it.Out.ObjectsFetched[1].Name, Is.EqualTo("Jane Doe"));
                    });
        }

        public class Input
        {
            public virtual IDbCommand SelectCommand { get; set; }
        }

        public class Output
        {
            public virtual T[] ObjectsFetched { get; set; }
        }

        public virtual BuildObject<T> BuildObject { get; set; }

        public override void Execute()
        {
            var objectList = new List<T>();

            using (var dataReader = In.SelectCommand.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    BuildObject.In.DataRecord = dataReader;
                    BuildObject.Execute();
                    var newObject = BuildObject.Out.Object;

                    objectList.Add(newObject);
                }
            }

            Out.ObjectsFetched = objectList.ToArray();
        }
    }
}
