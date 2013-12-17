using System;
using System.Data.SqlClient;
using NUnit.Framework;
using Simpler.Data;
using Simpler.Data.Tasks;

namespace Simpler
{
    public class OldAsk : T
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

    public class Ask : IO<Ask.Ins, Ask.Outs>
    {
        public class Ins
        {
            public string Question { get; set; }
        }

        public class Outs
        {
            public string Answer { get; set; }
        }

        public override void Execute()
        {
            Out.Answer =
                In.Question == "Is this cool?"
                    ? "Definitely."
                    : "Get a life.";
        }
    }

    public class LogAttribute : EventsAttribute
    {
        public override void BeforeExecute(T task)
        {
            Console.WriteLine(String.Format("{0} started.", task.Name));
        }

        public override void AfterExecute(T task)
        {
            Console.WriteLine(String.Format("{0} finished.", task.Name));
        }

        public override void OnError(T task, Exception exception)
        {
            Console.WriteLine(String.Format("{0} bombed; error message: {1}.", task.Name, exception.Message));
        }
    }

    [Log]
    public class BeAnnoying : I<BeAnnoying.Ins>
    {
        public class Ins
        {
            public int AnnoyanceLevel { get; set; }
        }

        // sub-task
        public Ask Ask { get; set; }

        public override void Execute()
        {
            // Notice that Ask was automatically instantiated.
            Ask.In.Question = "Is this cool?";

            for (var i = 0; i < In.AnnoyanceLevel; i++)
            {
                Ask.Execute();
            }
        }
    }

    public class Program
    {
        Program()
        {
            var beAnnoying = T.New<BeAnnoying>();
            beAnnoying.In.AnnoyanceLevel = 10;
            beAnnoying.Execute();
        }
    }

    public class Stuff
    {
        public string Name { get; set; }
    }

    public class OldFetchCertainStuff : IO<OldFetchCertainStuff.Ins, OldFetchCertainStuff.Outs>
    {
        public class Ins
        {
            public string SomeCriteria { get; set; }
        }

        public class Outs
        {
            public Stuff[] Stuff { get; set; }
        }

        public BuildParameters BuildParameters { get; set; }
        public FetchMany<Stuff> FetchStuff { get; set; }

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
                        AColumn as Name
                    from 
                        ABunchOfJoinedTables
                    where 
                        SomeColumn = @SomeCriteria
                        and
                        AnotherColumn = @SomeOtherCriteria
                    ";

                BuildParameters.In.Command = command;
                BuildParameters.In.Values = new {In.SomeCriteria, SomeOtherCriteria = "other criteria"};
                BuildParameters.Execute();

                FetchStuff.In.SelectCommand = command;
                FetchStuff.Execute();
                Out.Stuff = FetchStuff.Out.ObjectsFetched;
            }
        }
    }

    public class FetchCertainStuff : IO<FetchCertainStuff.Ins, FetchCertainStuff.Outs>
    {
        public class Ins
        {
            public string SomeCriteria { get; set; }
        }

        public class Outs
        {
            public Stuff[] Stuff { get; set; }
        }

        public override void Execute()
        {
            using(var connection = Db.Connect("MyConnectionString"))
            {
                const string sql =
                    @"
                    select 
                        AColumn as Name
                    from 
                        ABunchOfJoinedTables
                    where 
                        SomeColumn = @SomeCriteria
                        and
                        AnotherColumn = @SomeOtherCriteria
                    ";

                var values = new {In.SomeCriteria, SomeOtherCriteria = "other criteria"};

                Out.Stuff = Db.GetMany<Stuff>(connection, sql, values);
            }
        }
    }

    //[TestFixture]
    public class FetchCertainStuffTest
    {
        //[Test]
        public void should_return_9_pocos()
        {
            // Arrange
            var task = T.New<FetchCertainStuff>();
            task.In.SomeCriteria = "criteria";

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Out.Stuff.Length, Is.EqualTo(9));
        }
    }
}