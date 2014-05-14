using System;
using System.Data.SqlClient;
using NUnit.Framework;

namespace Simpler
{
    public class OldAsk : Task
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

    public class Ask : InOutTask<Ask.Input, Ask.Output>
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

    public class LogAttribute : EventsAttribute
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
    public class BeAnnoying : InTask<BeAnnoying.Input>
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
            var beAnnoying = Task.New<BeAnnoying>();
            beAnnoying.In.AnnoyanceLevel = 10;
            beAnnoying.Execute();
        }
    }
}