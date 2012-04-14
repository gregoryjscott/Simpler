#Simpler

You probably won't like Simpler.  If you enjoy spending your time configuring ORMs, interfacing with DI/IOC frameworks, generating code, and building complex domain models, then you will probably hate Simpler.  Simpler's primary goal is help developers create quick, simple solutions while writing the least amount of code possible.  Every piece of code in an application should have a clearly visible business purpose - the rest is just noise.

---

###"What is it?"

For the most part, Simpler is just a philosophy on .NET class design.  All classes that contain functionality are defined as Jobs.  A Job has optional input and/or outputs, along with a single Execute() method - and that's it.  Simpler comes with a Job base class, a static JobFactory class for instantiating Jobs, along with various built-in Jobs that you can use as sub-jobs in your Jobs.

    class AnswerUsingDynamicProperties : Job
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

Not a fan of the built-in dynamic Inputs and Outputs properties?  Fine - ignore them, create POCOs for your Job's input and outputs, and declare them as properties on your Job.

    class AnswerUsingStaticProperies : Job
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

###"What's the purpose of the JobFactory?"

JobFactory appears to just return an instance of the given Job type, but it actually returns a proxy to the Job.  The proxy allows for intercepting Job Execute() calls and performing actions before and/or after the Job execution.  For example, the built-in InjectSubJobs attribute will automatically instantiate sub-job properties (only if null) before Job execution, and automatically dispose of them after execution.

    [InjectSubJobs]
    class CompareAnswers : Job
    {
        // Sub-jobs
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
            var compareAnswers = JobFactory<CompareAnswers>.Create();
            compareAnswers.Execute();
            Console.WriteLine(compareAnswers.Outputs.AnswersMatch);
        }
    }

Sub-job injection simplifies the code, but more importantly it allows for mocking sub-jobs as necessary in Job unit tests.

---

###"What about database interaction?"

Simpler provides a small set of Jobs for interacting with System.Data.IDbCommand.  Using SQL, Simpler makes it pretty easy to get data out of a database and into .NET objects, or persist data from .NET objects to a database.

    [InjectSubJobs]
    class FetchSomeStuff : Job
    {
        // Inputs
        public string SomeCriteria { get; set; }

        // Outputs
        public SomeStuff[] SomeStuff { get; set; }

        // Sub-jobs (BuildParametersUsing<T> and FetchListOf<T> are built-in Simpler Jobs)
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

                // Use the SomeCriteria property value on this Job to build the @SomeCriteria parameter.
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

Simpler isn't a full-featured ORM, but it gets the job done.

---

In summary, Simpler is a tool for developing applications as sets of consistent, discrete, reusable Job classes.  I like to think of the Jobs as little source code building blocks that fit together nicely, but can easily be rearranged or modified as necessary.  Simpler works great in team environments because all developers on the team design classes using the same techniques and terminology, resulting in a consistent code base.  Simpler fits like a glove with a ASP.NET MVC (add a Jobs folder next to your Controllers, Models, and Views folders).  And finally, Simpler can be added to your .NET project in seconds using NuGet.

**Simpler is licensed under the MIT License.  A copy of the MIT license can be found in the LICENSE file.**
