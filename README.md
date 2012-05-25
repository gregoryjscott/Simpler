#Simpler

You probably won't like Simpler. If you enjoy spending your time configuring ORMs, interfacing with DI/IOC frameworks, generating code, and building complex domain models, then you will probably hate Simpler. Simpler's primary goal is help developers create quick, simple solutions while writing the least amount of code possible. Every piece of code in an application should have a clearly visible business purpose - the rest is just noise.

###"What is it?"

For the most part, Simpler is just a philosophy on .NET class design. All classes that contain functionality are defined as Tasks, named as verbs.  A Task has optional input and/or outputs (POCOs), along with a single Execute() method - and that's it.

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
    
Simpler comes with a Task base class, a static TaskFactory class for instantiating Tasks, along with various built-in Tasks that you can use as sub-tasks (a sub-task is just a Task property of another Task - see next example).

###"What's the purpose of the TaskFactory?"

TaskFactory appears to just return an instance of the given Task type, but it actually returns a proxy to the Task. The proxy allows for intercepting Task Execute() calls and performing actions before and/or after the Task execution. For example, the Simpler.Injection.InjectSubTasks attribute will automatically instantiate sub-tasks (only if null) before Task execution, and automatically dispose of them after execution.  Another common application is using custom attribute to integrate your favorite logging library.

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

Sub-task injection gives you just enough power to do testing by allowing for mocking sub-task behavior in tests.  No need for repository nonsense when it's only purpose is for testing.

###"What about database interaction?"

Simpler provides a small set of Simpler.Data.Tasks classes that simplify interacting with System.Data.IDbCommand. Using SQL, Simpler makes it pretty easy to get data out of a database and into POCOs, or persist data from a POCO to a database.

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

Simpler isn't a full-featured ORM, but it gets the job done.

###"Is it easy to test?"

    [TestFixture]
    public class FetchSomeStuffTest
    {
        [Test]
        public void should_return_9_pocos()
        {
            // Arrange
            var task = TaskFactory<FetchSomeStuff>.Create();

            // Act
            task.Execute();

            // Assert
            Assert.That(task.SomePocos.Length, Is.EqualTo(9));
        }
    }

By design, all classes clearly define their inputs, outputs, and code to test, so tests are very straightforward.

###"I just need to get things done well, will Simpler help?"

Simpler is a tool for developing applications as sets of consistent, discrete, interchangable classes that aren't THINGS, but rather DO THINGS. Simpler works great in team environments because everybody is designing classes with the same termnilogy, and any class can easily integrate with another. 

Need to fetch a list of contacts?  Create classes called FetchContactsTest and FetchContact and get to work.  That's Simpler. 

###"How do I install it?"
Nuget.  For writing tests, you will also need a testing library like nUnit or xUnit.net, and adding a mocking library such as Moq comes in handy.  All are available on Nuget.

###"Is Simpler is so simple it doesn't need documentation?"

That's what I'm thinking :).  I seriously hope to create some proper documentation at some point, but the coding is so much more fun.

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

###License
Simpler is licensed under the MIT License.  A copy of the MIT license can be found in the LICENSE file.
