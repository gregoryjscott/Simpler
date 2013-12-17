#Simpler

Simpler's goal is to help teams of developers complete complex projects with consistent and readable code.

For the most part, Simpler is just a philosophy on class design. In the traditional Object-oriented programming (OOP) approach, classes define objects that have data and methods with business logic. OOP classes are named as nouns that have verb methods that contain the business logic. The problem with this approach is that application operations typically need to interact with several classes, and it isn't always immediately clear where functionality that affects more than one object should reside. This often leads to "manager" classes or "service" classes that become bloated with logic that can't be assigned to one particular object class. Developers are constantly faced with decisions about where new logic should be placed, and conversely, it isn't obvious where to look for logic during maintenance.

The Simpler philosophy is to separate the data from the business logic. Data is defined in model classes that only contain properties. Models are typically just plain old CLR objects (POCOs), and therefore Simpler doesn't provide any functionality pertaining to models. Simpler is concerned with business logic.

##Tasks

Tasks are classes that store business logic. Tasks are named as verbs instead of nouns, and the name should clearly state the functionality provided by the task. A task class can have optional input properties and/or output properties, an `Execute()` method, and sub-tasks properties. That's all. Simpler provides a set of bases classes for defining tasks - `Task`, `InTask`, `OutTask`, and `InOutTask`.

###Task

The `Task` class is the base class for all task classes. It contains an abstract `Execute()` method, `Name` property, `Stats` property, and a static `Task.New<TTask>()` method that serves as a factory method for instantiating tasks.

```c#
public class WriteName : Task
{
    public override void Execute()
    {
        Console.WriteLine("{0} here.", Name);
    }
}

public class WriteNameProgram
{
    WriteNameProgram()
    {
        var writeName = Task.New<WriteName>();
        writeName.Execute(); // outputs "WriteName here."
        Console.WriteLine(writeName.Stats.ExecuteCount); // outputs "1"
    }
}
```

The `Name` property provides a read-only name based on the name of the task class itself. `Name` is often useful for logging task information. 

The `Stats` property keeps track of how many times the task is executed and the execute durations. `Stats` are useful for profiling, but they are also handy for testing scenarios when you need to assert that a given task or sub-task has been executed as expected.

Input and/or output data can be passed in and out of `Task` classes using properties. However, the `InTask`, `OutTask`, and `InOutTask` bases classes better fit this purpose.

###InTask

The `InTask` is a task that will apply business logic to its given input but will not produce any output. It requires a generic parameter type that defines the type of input to the task. The input is exposed to the  `Execute()` method through the `In` property. All references to inputs within the `Execute()` method are prefixed with `In.` therefore enhancing the readability of the business logic code.

A common convention is to define an `Input` class inside the `InTask` that contains the input as properties and pass the Input class as the generic parameter type to the `InTask`. This effectively makes the `In` property a container for all the necessary input to the task.

```c#
public class TellSecret : InTask<TellSecret.Input>
{
    public class Input
    {
        public string Secret { get; set; }
    }

    public override void Execute()
    {
        // Do something with secret.
    }
}
```

###OutTask

The `OutTask` is a task that will produce output using business logic but will not accept any input. It requires a generic parameter type that defines the type of output of the task. The output is set in the `Execute()` method using the `Out` property. All references to the outputs within the `Execute()` method are prefixed with `Out.` therefore enhancing the readability of the business logic code.

A common convention is to define an `Output` class inside the `OutTask` that contains the output as properties and pass the `Output` class as the generic parameter type to the `OutTask`. This effectively makes the `Out` property a container for all the output of the task.

```c#
public class ReceiveAdvice : OutTask<ReceiveAdvice.Output>
{
    public class Output
    {
        public string Advice { get; set; }
    }

    public override void Execute()
    {
        // Find some good advice.
    }
}
```

###InOutTask

The `InOutTask` task combines `InTask` with `OutTask`. It requires two generic parameter types, the first defining the type of input and the second defining the type of output.

```c#
public class AskDumbQuestion : InOutTask<AskDumbQuestion.Input, AskDumbQuestion.Output>
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
```

###Sub-task Injection

Typically business logic will need to be broken up into multiple task classes, and often it makes sense for a task to execute other tasks. The situation where one task relies on the instance of another task creates a dependency. To prevent tight coupling between the tasks in this situation, Simpler provides automatic sub-task injection.

Tasks define their task dependencies as properties and these are referred to as sub-tasks. A sub-task is no different from a normal task; it is only called a sub-task when it is defined as a property on another task. Before a task is executed, Simpler checks to see if the task has any sub-tasks. If sub-tasks are found to be null, Simpler will automatically create the subtasks and inject them into the task properties.

```c#
public class BeAnnoying : InTask<BeAnnoying.Input>
{
    public class Input
    {
        public int AnnoyanceLevel { get; set; }
    }

    // Sub-task
    public AskDumbQuestion AskDumbQuestion { get; set; }

    public override void Execute()
    {
        // Notice that AskDumbQuestion was automatically instantiated.
        AskDumbQuestion.In.Question = "Is this cool?";

        for (var i = 0; i < In.AnnoyanceLevel; i++)
        {
            AskDumbQuestion.Execute();
        }
    }
}
```

Sub-task injection keeps tasks loosely coupled and provides for advanced scenarios where dependencies need to be injected at runtime. More commonly, dependency injection is used to testing purposes.

##Testing

By design, all tasks clearly define their inputs, outputs, and code to test, therefore writing tests for most tasks is pretty straightforward.

```c#
[TestFixture]
public class CheckSnowReportTest
{
    [Test]
    public void should_find_snow_Xmas_1982()
    {
        // Arrange
        var checkSnowReport = Task.New<CheckSnowReport>();
        checkSnowReport.In.DateTime = new DateTime(1982, 12, 25);

        // Act
        checkSnowReport.Execute();

        // Assert
        Assert.That(checkSnowReport.Out.InchesOfSnow, Is.GreaterThan(0));
    }
}
```

###Fake

In some cases it is necessary to control the behavior of a tasks's sub-tasks (often referred to as mocking) in order to properly isolate and test a particular piece of logic. `Fake.Task<TTask>()` allows for overriding a task's `Execute()` logic  and can be used to change the behavior of a sub-task.

```c#
public class GoSkiing : OutTask<GoSkiing.Output>
{
    public class Output
    {
        public bool Yes { get; set; }
    }

    public CheckSnowReport CheckSnowReport { get; set; }

    public override void Execute()
    {
        CheckSnowReport.Execute();
        Out.Yes = CheckSnowReport.Out.InchesOfSnow >= 6;
    }
}

[TestFixture]
public class GoSkiingTest
{
    [Test]
    public void should_go_skiing_on_powder_days()
    {
        // Arrange
        var goSkiing = Task.New<GoSkiing>();
        goSkiing.CheckSnowReport = Fake.Task<CheckSnowReport>(csr => csr.Out.InchesOfSnow = 6);

        // Act
        goSkiing.Execute();

        // Assert
        Assert.That(goSkiing.Out.Yes, Is.True);
    }
}
```

`Fake.SubTasks()` will override the behavior of all sub-tasks with an `Execute()` that does nothing. For tasks with many sub-tasks, it's easier to start with a call to `Fake.SubTasks()` and then fake the individual tasks that are necessary for the particular test scenario.

##Data

Simpler isn't a full-featured object relational mapping (ORM) tool. ORMs work great in many scenarios, and Simpler tasks can certainly execute ORM commands. However, some projects require communicating with databases with crazy relational data and custom SQL becomes the most optimal way of interaction. Simpler provides functionality for interacting with relational databases using raw SQL.

Simpler comes with a set of tasks that could be considered a micro-ORM. The tasks in the `Simpler.Data` namespace can be used to get data out of a database and into POCOs, or persist data from POCOs to a database using parameterized SQL. For convenience, Simpler provides a static `Db` class that wraps the `Simpler.Data` tasks.

###Db

`Db.Connect()` will create and return an open instance of `System.Data.IDbConnection` using the given connection name. Simpler will search the configuration file for a `connectionString` entry that matches the given connection name, and use it along with `System.Data.Common.DbProviderFactories` to create and open the connection.

`Db.GetMany<T>()` returns an array of `T` instances by using the given connection and SQL to query the database for rows of data. If values object is provided, it will search the SQL for parameters and use the properties on the values object to create and set the parameter values.

```c#
public class FetchCertainStuff : InOutTask<FetchCertainStuff.Input, FetchCertainStuff.Output>
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
```

`Db.GetOne<T>()` returns one instance of `T` by using the given connection and SQL to query the database for a row of data. If values object is provided, it will search the SQL for parameters and use the properties on the values object to create and set the parameter values.

`Db.GetResult()` uses the given connection to execute the given SQL on the database and returns an integer result (usually the number of rows affected). If values object is provided, it will search the SQL for parameters and use the properties on the values object to create and set the parameter values.

`Db.GetScalar()` uses the given connection to execute the given SQL on the database and returns an object result. If values object is provided, it will search the SQL for parameters and use the properties on the values object to create and set the parameter values.

##Parallel

`Parallel` provide syntactic sugar on top of the `System.Threading.Parallel` for executing tasks in parallel. `Parallel.Execute()` will execute a variable number of tasks in parallel and return after all tasks have been executed.

```c#
public class FindFlight : InOutTask<FindFlight.Input, FindFlight.Output>
{
    public class Input
    {
        public FlightInfo FlightInfo { get; set; }
    }

    public class Output
    {
        public string Url { get; set; }
    }

    public CheckUnited CheckUnited { get; set; }
    public CheckDelta CheckDelta { get; set; }

    public override void Execute()
    {
        CheckUnited.In.FlightInfo = In.FlightInfo;
        CheckDelta.In.FlightInfo = In.FlightInfo;

        Parallel.Execute(CheckUnited, CheckDelta);

        Out.Url = CheckUnited.Out.Price < CheckDelta.Out.Price 
            ? CheckUnited.Out.Url 
            : CheckDelta.Out.Url;
    }
}
```

##Events

`Task.New<TTask>()` appears to just return an instance of the given `TTask`, however, it actually returns a proxy to the task. The proxy allows for intercepting `Execute()` calls to perform actions before and/or after the task executes. The `EventAttribute` can be sub-classed and applied to tasks to address cross-cutting concerns like logging.

```c#
public class LogAttribute : EventsAttribute
{
    public override void BeforeExecute(Task task)
    {
        Console.WriteLine("{0} started.", task.Name);
    }

    public override void AfterExecute(Task task)
    {
        Console.WriteLine("{0} finished.", task.Name);
    }

    public override void OnError(Task task, Exception exception)
    {
        Console.WriteLine("{0} bombed.", task.Name);
    }
}

[Log]
public class LogThings : Task
{
    // outputs "LogThings started." before Execute starts
    public override void Execute()
    {
        Console.WriteLine("{0} executing.", Name); // outputs "LogThings executing."
    }
    // outputs "LogThings finished." after Execute finishes
}
```

##Conclusion

Simpler eliminates the need for complex domain models, ORM configuration, generating code, and interfacing with DI/IoC frameworks. Developers don't waste time making decisions about class design. Need to fetch a list of contacts? Create the classes `FetchContactsTest` and `FetchContact` and get to work. That's Simpler.




