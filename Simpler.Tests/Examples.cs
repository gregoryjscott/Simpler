using System.Data.SqlClient;
using NUnit.Framework;
using Simpler.Data.Tasks;
using Simpler.Injection;
using Simpler.Tests.Mocks;

namespace Simpler
{
    class Ask : Task
    {
        // Inputs
        public string Question { get; set; }

        // Outputs
        public string Answer { get; private set; }

        public override void Execute()
        {
            Answer =
                Question == "Is this cool?"
                ? "Definitely."
                : "Get a life.";
        }
    }

    [InjectSubTasks]
    class BeAnnoying : Task
    {
        // Sub-tasks
        public Ask Ask { get; set; }

        public override void Execute()
        {
            // Notice that Ask was injected.
            Ask.Question = "Is this cool?";

            for (int i = 0; i < 10; i++)
            {
                Ask.Execute();
            }
        }
    }

    class Program
    {
        Program()
        {
            var beAnnonying = TaskFactory<BeAnnoying>.Create();
            beAnnonying.Execute();
        }
    }

    class SomePoco
    {
        public bool AmIImportant { get; set; }
    }

    [InjectSubTasks]
    class FetchSomeStuff : Task
    {
        // Inputs
        public string SomeCriteria { get; set; }

        // Outputs
        public SomePoco[] SomePocos { get; set; }

        // Sub-tasks
        public BuildParametersUsing<FetchSomeStuff> BuildParameters { get; set; }
        public FetchListOf<SomePoco> FetchList { get; set; }

        public override void Execute()
        {
            using (var connection = new SqlConnection("MyConnectionString"))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText =
                    @"
                    select 
                        SomeStoredBit as AmIImportant
                    from 
                        ABunchOfJoinedTables
                    where 
                        SomeColumn = @SomeCriteria 
                    ";

                // Use the SomeCriteria property value on this Task to build the @SomeCriteria parameter.
                BuildParameters.CommandWithParameters = command;
                BuildParameters.ObjectWithValues = this;
                BuildParameters.Execute();

                FetchList.SelectCommand = command;
                FetchList.Execute();
                SomePocos = FetchList.ObjectsFetched;
            }
        }
    }

    [TestFixture]
    public class FetchSomeStuffTest
    {
        [Test]
        public void should_return_9_pocos()
        {
            // Arrange
            var task = TaskFactory<FetchSomeStuff>.Create();
            task.SomeCriteria = "whatever";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.SomePocos.Length, Is.EqualTo(9));
        }
    }
}
