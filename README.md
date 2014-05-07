#Simpler

At its core, Simpler is a philosophy on or a pattern for .NET class design. Simpler can help developers&mdash;especially teams of developers&mdash;build complex projects with consistent, readable, and interchangeable/integrateable classes. 

Its primary goal is to help developers create quick, simple solutions while writing the least amount of code possible.

##Key Benefits of Simpler
- Eliminate the need to discuss class design, especially when working on a team 
- Make the code more understandable, consistent, and readable—especially on complex projects
- Simplify writing unit tests
- Makes maintenance easier as finding business logic, especially business logic that interacts with multiple classes, is simpler 

##The Simpler Philosophy
In the traditional Object-oriented Programming (OOP) approach, classes define objects (named with nouns) and include data and business logic (methods named with verbs). 

![OOP Diagram](/images/OOPDiagram.png)

But when an application needs to interact with several classes, it isn’t always clear where that business logic should be stored. So “manager” or “service” classes are often created for all of the business logic affecting multiple object classes. 

With OOP, developers must constantly make decisions about where to place new business logic, a problem which is exacerbated by complex projects or when working with a team. Even with careful class design, it’s not always obvious where to find particular business logic, especially when maintaining code.

However, with Simpler, data and business logic are divided into small, discrete “building blocks.” Although you’ll have a lot of small classes—instead of a few, very large classes—these classes can easily integrate with each other without “manager” or “service” classes. 

Simpler also separates data from business logic, with data defined in Model classes and business logic in Task classes. 

![Simpler Diagram](/images/SimplerDiagram.png)

Model classes are typically just plain old CLR objects (POCOs) and only contain properties. But a Task *does things*; each Task class is the equivalent of a discrete action. Simpler provides functionality for Tasks.

##Using Simpler



For the most part, Simpler is just a philosophy on .NET class design. All classes that contain functionality are defined as Tasks named as verbs. A Task has optional input and/or output, a single Execute() method, and possibly some sub-tasks - that's it.

```c#
public class Ask : Task
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
```
    
Simpler 2 adds some additional base classes, InTask, OutTask, and InOutTask, that allow for explicity defining the input and/or output of the Task.

```c#
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
```

Input is available to the Execute() method by way of the In property, and output is set using the Out property. This eliminates the need to comment your input and output properties, and makes it easy to identify the input and output within the Execute() method since all input is wrapped by In, and all output is set on Out.

###"How do I use it?"

First, you build a Task class. You then instantiate Tasks using the Task.New<T>() method.

Task.New<T>() appears to just return an instance of the given Task type. However, it actually returns a proxy to the Task. The proxy allows for intercepting Task Execute() calls to perform actions before and/or after the Task executes. Simpler uses this to automatically inject sub-task properties (only if null) before Task execution by way of the Simpler.EventsAttribute. Another common use of this functionality is to build a custom EventsAttribute to log task activity.

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

    public Ask Ask { get; set; }

    public override void Execute()
    {
        // "BeAnnoying started." was logged to the console before Execute() began.

        // Notice that Ask was automatically instantiated.
        Ask.In.Question = "Is this cool?";

        for (var i = 0; i < In.AnnoyanceLevel; i++)
        {
            Ask.Execute();
        }

        // "BeAnnoying finished." will be logged to the console after Execute() finishes.
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

That pretty much sums it up. You build task classes, you use Task.New<T> to create them, and you can use the power of the proxy to address cross cutting concerns like logging.

###"What about database interaction?"

Simpler provides a small set of Simpler.Data.Tasks classes that simplify interacting with System.Data.IDbCommand. Using SQL, Simpler makes it pretty easy to get data out of a database and into POCOs, or persist data from a POCO to a database.

```c#
public class Stuff
{
    public string Name { get; set; }
}

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
```

Simpler 2 adds a new Simpler.Data.Db static class that eliminates most of the boilerplate code.

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

The Db class also offers GetOne<T>(), GetResult() and GetScalar() methods. Simpler isn't a full-featured ORM, but for most scenarios it gets the job done.

###"Is it easy to test?"

```c#
[TestFixture]
public class FetchCertainStuffTest
{
    [Test]
    public void should_return_9_pocos()
    {
        // Arrange
        var task = TaskFactory<FetchCertainStuff>.Create();
        task.In.SomeCriteria = "whatever";

        // Act
        task.Execute();

        // Assert
        Assert.That(task.Out.Stuff.Length, Is.EqualTo(9));
    }
}
```

By design, all Tasks clearly define their inputs, outputs, and code to test, so tests are very straightforward.

A Task's dependencies are its inputs, outputs, and sub-tasks. The automatic sub-task injection provides the power to do testing by allowing for mocking sub-task behavior. This eliminates the need for repository nonsense when the only purpose is for testing.

###"I just need to get things done well, will Simpler help?"

Simpler is a tool for developing applications as sets of consistent, discrete, interchangable classes that aren't THINGS, but rather DO THINGS. Simpler works great in team environments because everybody is designing classes with the same termnilogy, and any class can easily integrate with another. 

Develpers don't waste time making decisions about class design. Need to fetch a list of contacts? Create classes called FetchContactsTest and FetchContact and get to work. That's Simpler. 

###"How do I install it?"

Use Nuget. Simpler works with .NET 3.5 and above.

###"Is Simpler so simple it doesn't need documentation?"

Exactly. I seriously hope to create some proper documentation at some point, but the coding is so much more fun.

###Acknowledgments

The following have contributed in some way and/or have built something awesome with Simpler.

- [bobnigh](https://github.com/bobnigh)
- [Clancey](https://github.com/Clancey)
- [corys](https://github.com/corys)
- [Crosis](https://github.com/Crosis)
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
