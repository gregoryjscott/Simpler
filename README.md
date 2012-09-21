#Simpler

You probably won't like Simpler. If you enjoy spending your time configuring ORMs, interfacing with DI/IOC frameworks, generating code, and building complex domain models, then you will probably hate Simpler. Simpler's primary goal is help developers create quick, simple solutions while writing the least amount of code possible. Every piece of code in an application should have a clearly visible business purpose - the rest is just noise.

###"What is it?"

For the most part, Simpler is just a philosophy on .NET class design. All classes that contain functionality are defined as Tasks, named as verbs. A Task has optional input and/or output, a single Execute() method, and possibly some sub-taskss - that's it.

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
    
Simpler 2 adds some additional base classes, InTask, OutTask, and InOutTask, that allow for explicity defining the input and/or output of the Task.

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

Input is available to the Execute method by way of the In property, and output is set using the Out property. This eliminates the need to comment your input/output properties, and makes it easy to identify the input/output within the Execute method since all input is wrapped by In, and all output is set on Out.

###"How do I use it?"

You create Tasks using the Task.New<T>() method, which appears to just return an instance of the given Task type. However, it actually returns a proxy to the Task. The proxy allows for intercepting Task Execute() calls and performing actions before and/or after the Task executes. Simpler uses this to automatically inject sub-task properties (only if null) before Task execution by way of the Simpler.EventsAttribute. Another common use of this functionality is to build a custom EventsAttribute to log task activity.

	```c#
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
	```

A Task's dependencies are it's inputs, outputs, and sub-tasks. The sub-task injection provides the power to do testing by allowing for mocking sub-task behavior. This eliminates the need for repository nonsense when the only purpose is for testing.

###"What about database interaction?"

Simpler provides a small set of Simpler.Data.Tasks classes that simplify interacting with System.Data.IDbCommand. Using SQL, Simpler makes it pretty easy to get data out of a database and into POCOs, or persist data from a POCO to a database.

    class SomePoco 
    {
        public bool AmIImportant { get; set; }
    }

    public class FetchSomeStuff : InOutTask<FetchSomeStuff.Input, FetchSomeStuff.Output>
    {
        public class Input
        {
            public string SomeCriteria { get; set; }
        }

        public class Output
        {
            public SomePoco[] SomePocos { get; set; }
        }

        public BuildParameters BuildParameters { get; set; }
        public FetchMany<SomePoco> FetchList { get; set; }

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

                // Use the In.SomeCriteria property value on this Task to build the @SomeCriteria parameter.
                BuildParameters.In.Command = command;
                BuildParameters.In.Values = In;
                BuildParameters.Execute();

                FetchList.In.SelectCommand = command;
                FetchList.Execute();
                Out.SomePocos = FetchList.Out.ObjectsFetched;
            }
        }
    }

Simpler 2 adds a new Db static class that eliminates most of the boiler plate code.

    public class FetchSomeStuff : InOutTask<FetchSomeStuff.Input, FetchSomeStuff.Output>
    {
        public class Input
        {
            public string SomeCriteria { get; set; }
        }

        public class Output
        {
            public SomePoco[] SomePocos { get; set; }
        }

        public override void Execute()
        {
            using(var connection = Db.Connect("MyConnectionString"))
            {
                const string sql = 
                    @"
                    select 
                        SomeStoredBit as AmIImportant
                    from 
                        ABunchOfJoinedTables
                    where 
                        SomeColumn = @SomeCriteria 
                    ";

                Out.SomePocos = Db.GetMany<SomePoco>(connection, sql, In);
            }
        }
    }

Simpler isn't a full-featured ORM, but for most scenarios it gets the job done.

###"Is it easy to test?"

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

By design, all Tasks clearly define their inputs, outputs, and code to test, so tests are very straightforward.

###"I just need to get things done well, will Simpler help?"

Simpler is a tool for developing applications as sets of consistent, discrete, interchangable classes that aren't THINGS, but rather DO THINGS. Simpler works great in team environments because everybody is designing classes with the same termnilogy, and any class can easily integrate with another. 

Develpers don't waste time making decisions about class design. Need to fetch a list of contacts? Create classes called FetchContactsTest and FetchContact and get to work. That's Simpler. 

###"How do I install it?"

Use Nuget. Simpler works with .NET 3.5 and above.

###"Is Simpler so simple it doesn't need documentation?"

Exactly. I seriously hope to create some proper documentation at some point, but the coding is so much more fun.

###Acknowledgments

The following have contributed in some way, and have built something awesome with Simpler.

- [bobnigh](https://github.com/bobnigh)
- [Clancey](https://github.com/Clancey)
- [corys](https://github.com/corys)
- [danvanorden](https://github.com/danvanorden)
- [dchristine](https://github.com/dchristine)
- [jkettell](https://github.com/jkettell)
- [JOrley](https://github.com/JOrley)
- [jshoemaker](https://github.com/jshoemaker)
- [ralreegorganon](https://github.com/ralreegorganon)
- [rodel-rdi](https://github.com/rodel-rdi)
- [sonhuilamson](https://github.com/sonhuilamson)
- [timrisi](https://github.com/timrisi)

###License
Simpler is licensed under the MIT License. A copy of the MIT license can be found in the LICENSE file.
