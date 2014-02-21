﻿using System;
using NUnit.Framework;
using Simpler.Data;

namespace Simpler
{
    public class OldAsk: Task
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

    public class Ask: InOutTask<Ask.Input, Ask.Output>
    {
        public class Input
        {
            public string Question { get; set; }
        }

        public class Output
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

    public class LogAttribute: EventsAttribute
    {
        public override void BeforeExecute(Task task)
        {
            Console.WriteLine(String.Format("{0} started.", task.Name));
        }

        public override void AfterExecute(Task task)
        {
            Console.WriteLine(String.Format("{0} finished.", task.Name));
        }

        public override void OnError(Task task, Exception exception)
        {
            Console.WriteLine(String.Format("{0} bombed; error message: {1}.", task.Name, exception.Message));
        }
    }

    [Log]
    public class BeAnnoying: InTask<BeAnnoying.Input>
    {
        public class Input
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
            Execute.Now<BeAnnoying>(ba => {
                ba.In.AnnoyanceLevel = 10;
            });
        }
    }

    public class Stuff
    {
        public string Name { get; set; }
    }

    public class FetchCertainStuff: InOutTask<FetchCertainStuff.Input, FetchCertainStuff.Output>
    {
        public class Input
        {
            public string SomeCriteria { get; set; }
        }

        public class Output
        {
            public Stuff[] Stuff { get; set; }
        }

        public override void Execute()
        {
            using (var connection = Db.Connect("MyConnectionString"))
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

                Out.Stuff = Db.Get<Stuff>(connection, sql, values);
            }
        }
    }

    //[TestFixture]
    public class FetchCertainStuffTest
    {
        //[Test]
        public void should_return_9_pocos()
        {
            var task = Execute.Now<FetchCertainStuff>(fcs => {
                fcs.In.SomeCriteria = "criteria";
            });

            Assert.That(task.Out.Stuff.Length, Is.EqualTo(9));
        }
    }
}
