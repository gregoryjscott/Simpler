using System;
using System.Data.SqlClient;
using Simpler.Data.Tasks;
using Simpler.Injection;

namespace Simpler
{
    class AnswerUsingDynamicProperties : Task
    {
        public override void Execute()
        {
            Outputs = new
            {
                Answer =
                    Inputs.Question == "Is this cool?"
                        ? "Definitely."
                        : "Get a life."
            };
        }
    }

    class AnswerUsingStaticProperies : Task
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
    class CompareAnswers : Task
    {
        // Sub-tasks
        public AnswerUsingDynamicProperties AnswerUsingDynamicProperties { get; set; }
        public AnswerUsingStaticProperies AnswerUsingStaticProperies { get; set; }

        public override void Execute()
        {
            const string question = "Is this cool?";

            // Notice that AnswerUsingDynamicProperties is already instantiated.
            AnswerUsingDynamicProperties.Inputs.Question = question;
            AnswerUsingDynamicProperties.Execute();

            // Notice that AnswerUsingStaticProperies is already instantiated.
            AnswerUsingStaticProperies.Question = question;
            AnswerUsingStaticProperies.Execute();

            Outputs = new
            {
                AnswersMatch =
                    AnswerUsingDynamicProperties.Outputs.Answer == AnswerUsingStaticProperies.Answer
            };
        }
    }

    class Program
    {
        Program()
        {
            var compareAnswers = TaskFactory<CompareAnswers>.Create();
            compareAnswers.Execute();
            Console.WriteLine(compareAnswers.Outputs.AnswersMatch);
        }
    }

    class SomeStuff {}

    [InjectSubTasks]
    class FetchSomeStuff : Task
    {
        // Inputs
        public string SomeCriteria { get; set; }

        // Outputs
        public SomeStuff[] SomeStuff { get; set; }

        // Sub-tasks (BuildParametersUsing<T> and FetchListOf<T> are built-in Simpler Tasks)
        public BuildParametersUsing<FetchSomeStuff> BuildParameters { get; set; }
        public FetchListOf<SomeStuff> FetchList { get; set; }

        public override void Execute()
        {
            using (var connection = new SqlConnection("MyConnectionString"))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText =
                    @"
                    SELECT 
                        SomeStuff
                    FROM 
                        ABunchOfJoinedTables
                    WHERE 
                        OneOfTheTables.SomeColumn = @SomeCriteria 
                    ";

                // Use the SomeCriteria property value on this Task to build the @SomeCriteria parameter.
                BuildParameters.CommandWithParameters = command;
                BuildParameters.ObjectWithValues = this;
                BuildParameters.Execute();

                // Fetch the list of SomeStuff and set the output property.
                FetchList.SelectCommand = command;
                FetchList.Execute();
                SomeStuff = FetchList.ObjectsFetched;
            }
        }
    }
}
