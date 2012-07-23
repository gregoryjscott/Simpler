#Simpler

You probably won't like Simpler.  If you enjoy spending your time configuring ORMs, interfacing with DI/IOC frameworks, generating code, and building complex domain models, then you will probably hate Simpler.  Simpler's primary goal is help developers create quick, simple solutions while writing the least amount of code possible.  Every piece of code in an application should have a clearly visible business purpose - the rest is just noise.

---

###"What is it?"

For the most part, Simpler is just a philosophy on .NET class design.  All classes that contain functionality are defined as Tasks.  A Task has optional input and/or outputs, along with a single Execute() method - and that's it.  Simpler comes with a Task base class, a static TaskFactory class for instantiating Tasks, along with various built-in Tasks that you can use as sub-tasks in your Tasks.

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

Not a fan of the built-in dynamic Inputs and Outputs properties?  Fine - ignore them, create POCOs for your Task's input and outputs, and declare them as properties on your Task.

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

---

###"What's the purpose of the TaskFactory?"

TaskFactory appears to just return an instance of the given Task type, but it actually returns a proxy to the Task.  The proxy allows for intercepting Task Execute() calls and performing actions before and/or after the Task execution.  For example, the built-in InjectSubTasks attribute will automatically instantiate sub-task properties (only if null) before Task execution, and automatically dispose of them after execution.

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

Sub-task injection simplifies the code, but more importantly it allows for mocking sub-tasks as necessary in Task unit tests.

---

###"What about database interaction?"

Simpler provides a small set of Tasks for interacting with System.Data.IDbCommand.  Using SQL, Simpler makes it pretty easy to get data out of a database and into .NET objects, or persist data from .NET objects to a database.

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

Simpler isn't a full-featured ORM, but it gets the task done.

---

In summary, Simpler is a tool for developing applications as sets of consistent, discrete, reusable Task classes.  I like to think of the Tasks as little source code building blocks that fit together nicely, but can easily be rearranged or modified as necessary.  Simpler works great in team environments because all developers on the team design classes using the same techniques and terminology, resulting in a consistent code base.  Simpler fits like a glove with a ASP.NET MVC (add a Tasks folder next to your Controllers, Models, and Views folders).  And finally, Simpler can be added to your .NET project in seconds using NuGet.

**Simpler is licensed under the MIT License.  A copy of the MIT license can be found in the LICENSE file.**
