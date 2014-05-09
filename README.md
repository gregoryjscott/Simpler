#Simpler

At its core, Simpler is a philosophy on or a pattern for .NET class design. Simpler can help developers—especially teams of developers—build complex projects using consistent, readable classes that can be easily integrated with each other. 

##Key Benefits of Simpler
- Eliminates the need to discuss and decide on class design 
- Makes the code more understandable, consistent, and readable
- Simplifies [writing unit tests] (#writing-tests)
- Provides a cleaner method of [addressing cross-cutting concerns] (#eventsattribute)
- Simplifies maintenance by making it easier to find business logic 

##The Simpler Philosophy
In the traditional Object-oriented Programming (OOP) approach, classes define objects (named with nouns) and include data and business logic (methods named with verbs). 

![OOP Diagram](/images/OOPDiagram.png)

But when an application needs to interact with several classes, it isn’t always clear where that business logic should be stored. So “manager” or “service” classes are often created for all of the business logic affecting multiple object classes. 

With OOP, developers must constantly make decisions about where to place new business logic, a problem which is exacerbated by complex projects or when working with a team. Even with careful class design, it’s not always obvious where to find particular business logic, especially when maintaining code.

However, with Simpler, data and business logic are divided into small, discrete “building blocks.” Although you’ll have a lot of small classes—instead of a few, very large classes—these classes can easily integrate with each other without “manager” or “service” classes. 

Simpler also separates data from business logic, with data defined in Model classes and business logic in Task classes. 

To make finding the Tasks and Models easy, you can organize them within the solution.

![Simpler Diagram](/images/SimplerDiagram.png)

Model classes are typically just plain old CLR objects (POCOs) and only contain properties. But a Task *does things*; each Task class is the equivalent of a discrete action. Simpler provides functionality for Tasks.

##Installing Simpler

Use Nuget. Simpler works with .NET 3.5 and above.

##Using Simpler
Using Simpler is, well, *simple*.  

1. [Create a Task class] (#creating_tasks). 
2. [Instantiate the Task] (#instantiating_tasks) using the Task.New() method. 
3. Execute the task. 

Simpler also provides some additional functionality: 

- From within a Task, [execute other Tasks (sub-tasks)] (#injecting-sub-tasks)
- [Perform actions before or after execute] (#eventsattribute), which is especially useful for addressing cross-cutting concerns such as logging
- Use [`Stats`] (#stats) for profiling or for testing scenarios based on a Task executing or executing within a duration
- Use [`Name`] (#name) for getting the name of the Task class, which is often useful for logging Task information
- When testing a Task with sub-tasks, isolate the Task logic by [mocking the sub-tasks] (#mocking)

###<a name="creating_tasks"></a>Creating Tasks

You should create a new Task when you will be inventing enough code that you’ll want to test it. For example, if logic can be done simply using LINQ, there’s no reason to create a Task just for that. 

When you create a Task, you should name it so that everyone can easily identify what the Task does. Follow these naming rules:
 
- Begin each Task name with a verb (because the Task is action, it’s doing something)
- Clearly state the Task’s purpose in the name

>**Example:** A Task that parses an XML file containing projects might be called *ParseProjectsXml*. But it shouldn’t be called *XmlProjectParser* or *XmlProject*.

Simpler provides 3 base classes for defining Tasks: 

- [`InTask`] (#intask) (applies business logic to an input but doesn’t produce any output)
- [`OutTask`] (#outtask) (produces output using business logic but does not accept any input)
- [`InOutTask`] (#inouttask) (applies business logic to an input and produces an output)

Simpler also includes a `Task` base class. This `Task` base class 

- Provides backwards compatibility for Simpler 1.0 
- Includes the static `Task.New<TTask>()` method, which is a factory method for [instantiating tasks] (#instantiating_tasks)
- Enables you to create a Task that has not inputs or outputs

In addition, all Tasks inherit  the [`Name`] (#name) property
and the [`Stats`] (#stats) property from this base class.

####<a name="intask"></a>InTask
An `InTask` applies business logic to an input but doesn’t produce an output. For example, an `InTask` might receive an input of set of new information and then insert that into a database. 

For an `InTask`, you must enter a generic parameter type that defines the type of input. This input is exposed to the `Execute()` method through the `In` property.

To make the `In` property a container for all input, you can define an `Input` class inside the `InTask`. This `Input` class contains the input as properties and is passed to the `InTask` as the generic parameter type. 

>**Note:** The following example also uses Simpler.Data.

```c#
public class InsertSighting : InTask<InsertSighting.Input>
{
    public class Input
    {
        public WildlifeSightings Sighting { get; set; }
    }

    public override void Execute()
    {
        using (var connection = Db.Connect("WildlifeSightings"))
        {
            const string sql = @"EXECUTE [dbo].[Insert_Sighting]
                    @SightingId = @SightingId
                    @UserId = @UserId
                    @SightingDate = @SightingDate,
                    @Species = @Species,
                    @Genus = @Genus;

            In.Sighting.SightingDate = DateTime.ParseExact(In.Sighting.SightingDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);

            var SightingId = In.Sighting.SightingId;
            var UserId = In.Sighting.UserId;
            var SightingDate = In.Sighting.SightingDate;
            var Species = In.Sighting.SightingFieldsList[0].Species;
            var Genus = In.Sighting.SightingFieldsList[0].Genus;

            var values = new { SightingId, UserId, SightingDate, Species, Genus };
            Db.GetResults(connection, sql, values);
        }
    }
}
```

####<a name="outtask"></a>OutTask

An `OutTask` has no input, but it uses business logic to produce an output. For example, an `OutTask` might query for a set of data and make the results available. 

For an `OutTask`, you must enter a generic parameter type that defines the type of output. This output is exposed in the `Execute()` method through the `Out` property. 

To make the `Out` property a container for all output, you can define an `Output` class inside the `OutTask`. This `Output` class contains the output as properties and is passed to the `OutTask` as the generic parameter type. 

>**Note:** The following example also uses Simpler.Data. 

```c#
public class FetchBirdSightings : OutTask<FetchBirdSightings.Output>
{
    public class Output
    {
        public BirdSighting[] BirdSightings { get; set; }
    }

    public override void Execute()
    {
        using (var connection = Db.Connect("WildlifeSightings"))
        {
            const string sql = @"
                select 
                    SightingDate as SightingDate,
                    Species as Species
                from
                    [Sight].[Birds]
                order by
                    SightingDate
                ";
            Out.BirdSightings = Db.GetMany<BirdSighting>(connection, sql);
        }
    }
}
```

####InOutTask

An `InOutTask` is a combination of an [`InTask`] (#intask) and an [`OutTask`] (#outtask). An `InOutTask` applies business logic to an input and produces an output. For example, an `InOutTask` might input some variables, which are used in a database query, and output to a list. 
 
For an `InOutTask`, you must enter 2 generic parameter types: one for the type of input and one for the type of output. As with an `InTask` and an `OutTask`, these parameters are exposed in the `Execute()` method through the `In` and `Out` properties respectively. You can also define `Input` and `Output` classes inside the `InOutTask`. Refer to [InTask] (#intask) and [OutTask] (#outtask) for additional information.

>**Note:** The following example also uses Simpler.Data.

```c#
public class FetchSightingsBySpeciesAndGenus : InOutTask<FetchSightingsBySpeciesAndGenus.Input, FetchSightingsBySpeciesAndGenus.Output>
{
    public class Input
    {
        public int SpeciesId { get; set; }
        public int GenusId { get; set; }
    }

    public class Output
    {
        public SightingDate[] Sightings { get; set; }
    }

    public override void Execute()
    {
        using (var connection = Db.Connect("WildlifeSightings"))
        {
            const string sql = @"
                EXECUTE [dbo].[Get_BirdSpecies]
                    @inSpeciesId = @SpeciesId
                    ,@inGenusId = @GenusId
                ";
                
                var values = new {In.SpeciesId, In.GenusId};

                Out.Sightings = Db.GetMany<SightingDate>(connection, sql,values);
        }
    }
}
```

###<a name="instantiating_tasks"></a>Instantiating Tasks

When you have [created a Task](#creating_tasks), you can instantiate it using the `Task.New()` method.

>**Note:** Task.New() appears to return an instance of the Task. However, it actually returns a proxy of the Task. This proxy allows Simpler to intercept Task Execute() calls to perform actions before and/or after the Task executes using the **EventsAttribute**.  

Do not use the `Task.New()` method to execute a Task from another Task. Instead, use [sub-task injection] (#injecting-sub-tasks). 

```c#
public class Program
{
    Program()
    {
        var insert = Task.New<InsertSighting>();
        insert.In.Sighting = wildlife;
        insert.Execute();
    }
}
```

##Additional Simpler Functionality

###<a name="injecting-sub-tasks"></a>Injecting Sub-tasks

With Simpler, a Task contains the smallest piece of useable functionality. Therefore, you’ll often need a Task to execute other Tasks, referenced as sub-tasks, which creates a dependency between the Tasks. But to prevent tight coupling between the Tasks, Simpler provides automatic sub-task injection. 

>**Note:** Sub-task injection also supports advanced scenarios, such as injecting dependencies at runtime. This type of injection is typically used for testing purposes. 

To inject sub-tasks within a Task class, define the sub-tasks as properties. Any Task can be referenced as a sub-task. A sub-task is no different from a normal Task—it’s only called a sub-task when it’s defined as a property on another Task. 

Before executing a Task, Simpler checks whether the Task has any sub-tasks. If so, Simpler automatically creates the sub-tasks and injects them into the Task properties. 

```c#
public class AddSightingTimestamp : InTask<AddSightingTimestamp.Input>
{
    public class Input
    {
        public string Directory { get; set; }
    }

    public GetSightingFiles GetSightingFiles { get; set; }

    public override void Execute()
    {
        string timestamp = DateTime.Now.ToString("yyyyMMdd_");

        GetFiles.In.Directory = In.Directory;
        GetFiles.Execute();
        SightingDateFile[] files = GetFiles.Out.Files;

        foreach (var file in files)
        {
            string fileName = file.FileName;
            string directory = file.FileDirectory

            File.Move(directory + fileName, directory + timestamp + fileName);
        }
    }
}
```

###<a name="eventsattribute"></a>Performing Actions Before or After Execute

When you use the [`Task.New<TTask>()`] (#instantiating_tasks) method, it returns a proxy to the Task, which enables Simpler to intercept Task `Execute()` calls using the `EventsAttribute`. When the `Execute()` call is intercepted, Simpler can perform actions before or after the Task executes or when the Task errors. 

This intercepting is especially useful for addressing cross-cutting concerns, such as logging, as the `EventsAttribute` can be sub-classed and easily applied to Tasks. 

The following example shows a custom `EventsAttribute` for logging Task activity.  

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
```

After you build the `EventsAttribute` code, include a reference to it in every Task that you want to use the `EventsAttribute`.  

```c#
[Log]
public class FetchBirdSightings : OutTask<FetchBirdSightings.Output>
{
    public class Output
    {
        public BirdSighting[] BirdSightings { get; set; }
    }

    public override void Execute()
    {
        // "FetchBirdSightings started." was logged to the console before Execute() began.
        using (var connection = Db.Connect("WildlifeSightings"))
        {
            const string sql = @"
                select 
                    SightingDate as SightingDate,
                    Species as Species
                from
                    [Sight].[Birds]
                order by
                    SightingDate
                ";
            Out.BirdSightings = Db.GetMany<BirdSighting>(connection, sql);
        // "FetchBirdSightings finished." will be logged to the console after Execute() finishes. 
        }
    }
}
```

###Using Stats and Name Properties 

The `Stats` and `Name` properties, which all Tasks inherit from the `Task` base class, are typically used in test or logging. 

####<a name="stats"></a>Stats

A Task's `Stats` property tracks how many times the Task is executed and the execute durations. `Stats` are useful for profiling as well as for testing scenarios when you need to assert that a Task or sub-task has been executed as expected. 

Refer to the [Writing Tests example] (#writing-tests) to see the `Stats` property in use.

####<a name="name"></a>Name

Because [`Task.New<TTask>()` returns a proxy] (#instantiating_tasks) to the Task, with *proxy* appended to the  Task name. To get a read-only name based on the Task class itself, use the `Name` property. 

Refer to the [`EventsAttribute` example] (#eventsattribute) to see the `Name` property in use.  

##<a name="writing-tests"></a>Writing Tests with Simpler

By design, Simpler forces you to create code that is easy to test. Each Task clearly defines its inputs, outputs, and the discrete code to test, so writing a test is typically straightforward. Simpler works particularly well in a [TDD workflow] (#tdd-example). 

Simpler also includes functionality to make writing tests easier: 

- If your Task includes sub-tasks, isolate the logic of the Task being tested by [mocking the sub-task behavior] (#mocking) using `Fake.Task<TTask>()`. 
- Use the [`Stats`] (#stats) property to test scenarios such as asserting that a Task has executed as expected or within a expected duration.
- Use the [`Name`] (#name) property to get a read-only name of the Task class. 

```c#
[TestFixture]
public class FetchSightingsBySpeciesAndGenusTest
{
    [Test]
    public void executes_in_acceptable_range()
    {
        // Arrange
        var task = Task.New<FetchSightingsBySpeciesAndGenus>();
        task.In.SpeciesId = "grisgena";
        task.In.GenusId = "Podiceps";

        // Act
        task.Execute();

        // Assert
        Assert.That(task.Stats.ExecuteDurations, Is.LessThan(3));
    }
}
```

###<a name="mocking"></a>Mocking

When writing a test for a Task with an [injected sub-task] (#injecting-sub-tasks), you may want to isolate the test to only the Task’s business logic. Using `Fake.Task<TTask>()`, you override the sub-task’s `Execute()` logic and instead define the data you want the test to “mock” being provided by the sub-task. 

If the Task has many sub-tasks, it’s easier to begin with a call to `Fake.SubTasks()` and then fake the individual tasks that are needed for the specific test scenario. 

####Example

Assume the following Task: 

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
```

A unit test using `Fake.Task<TTask>()` might look like the following: 

```c#
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

###<a name="tdd-example"></a>TDD Example Workflow

Simpler is especially suited to using a TDD workflow, as shown in the following example.

1. Identify the inputs you need to supply, the logic to be performed, and the exact output you expect. 

 ```c#
 public class AddNumbers : InOutTask<AddNumbers.Input, AddNumbers.Output>
 {
     public class Input
     {
         public int FirstNumber { get; set; }
         public int SecondNumber { get; set; }
     }
 
     public class Output
     {
         public int Sum { get; set; }
     }

     public override void Execute()
     {
         throw new NotImplementedException();
     }
 }
 ```

2. Write a test. 

 ``` c#
 [TestFixture]
 public class AddNumbersTest
 {
     [Test]
     public void should_work()
     {
         var task = Task.New<AddNumbers>();
         task.In.FirstNumber = 2;
         task.In.SecondNumber = 3;
         task.Execute();

         Assert.That(task.Out.Sum, Is.EqualTo(5));
         }
 }
 ```

3. Verify the test fails. 

 ```
 .F...................................................
 Tests run: 52, Errors: 1, Failures: 0, Inconclusive: 0, Time: 5.8431684 seconds
 Not run: 0, Invalid: 0, Ignored: 0, Skipped: 0

 Errors and Failures:
 1) Test Error : Simpler.AddNumbersTest.should_work
    System.NotImplementedException : The method or operation is not implemented.
    at Simpler.AddNumbers.Execute() in C:\Users\greg\Projects\Simpler\app\Simpler.Tests\Examples.cs:line 171
    at Castle.DynamicProxy.AbstractInvocation.Proceed()
    at Simpler.Core.Tasks.ExecuteTask.Execute() in C:\Users\greg\Projects\Simpler\app\Simpler\Core\Tasks\ExecuteTask.cs:line 56
    at Simpler.Core.Tasks.CreateTask.<Execute>b__0(IInvocation invocation) in C:\Users\greg\Projects\Simpler\app\Simpler\Core\Tasks\CreateTask.cs:line 33
    at Simpler.Core.ExecuteInterceptor.Intercept(IInvocation invocation) in C:\Users\greg\Projects\Simpler\app\Simpler\Core\ExecuteInterceptor.cs:line 19
    at Castle.DynamicProxy.AbstractInvocation.Proceed()
    at Simpler.AddNumbersTest.should_work() in C:\Users\greg\Projects\Simpler\app\Simpler.Tests\Examples.cs:line 184
 ```

4. Implement the business logic in the Task. 

 ```c#
 public class AddNumbers : InOutTask<AddNumbers.Input, AddNumbers.Output>
 {
     public class Input
     {
         public int FirstNumber { get; set; }
         public int SecondNumber { get; set; }
     }

     public class Output
     {
         public int Sum { get; set; }
     }

     public override void Execute()
     {
         Out.Sum = In.FirstNumber + In.SecondNumber;
     }
 }
 ```

5. Verify the Task passes the test. 

 ```
 ....................................................
 Tests run: 52, Errors: 0, Failures: 0, Inconclusive: 0, Time: 5.237 seconds
  Not run: 0, Invalid: 0, Ignored: 0, Skipped: 0
 ```

##Contributing

See [Contributing](CONTRIBUTING.md).

##Simpler HOF

The following individuals are in the Simpler Hall of Fame.

- [bobnigh](https://github.com/bobnigh) (pre, 2.\*)
- [Clancey](https://github.com/Clancey) (contributor, 1.\*)
- [corys](https://github.com/corys) (2.\*)
- [Crosis](https://github.com/Crosis) (2.\*)
- [danvanorden](https://github.com/danvanorden) (1.\*)
- [dchristine](https://github.com/dchristine) (1.\*, 2.\*)
- [jkettell](https://github.com/jkettell) (pre, 2.\*)
- [JOrley](https://github.com/JOrley) (1.\*)
- [jshoemaker](https://github.com/jshoemaker) (pre)
- [kamillf](https://github.com/kamillf) (contributor)
- [Pete Rose](http://en.wikipedia.org/wiki/Pete_Rose) (4,256 hits)
- [ralreegorganon](https://github.com/ralreegorganon) (contributor)
- [rlgnak](https://github.com/rlgnak) (contributor, 2.\*)
- [rodel-rdi](https://github.com/rodel-rdi) (1.\*)
- [sonhuilamson](https://github.com/sonhuilamson) (1.\*, 2.\*)
- [timrisi](https://github.com/timrisi) (2.\*)

###Legend

**pre** - Worked on a project that inspired Simpler, which included the Task class with sub-task injection. [bobnigh](https://github.com/bobnigh) was a co-author of the original Task class.

**1.*** - Worked on projects that used versions of Simpler 1, trusted a crazy idea, provided feedback, and ultimately proved it worked.

**2.*** - Was a teammate of mine on projects that used (many) beta versions of Simpler 2. That the Simpler 2 API turned out nice is only because these individuals provided great feedback while trying to build real projects with real budgets at the same time.

**contributor** - Has contributed to Simpler in some meaningful way.

##License
Simpler is licensed under the MIT License. A copy of the MIT license can be found in the LICENSE file.
