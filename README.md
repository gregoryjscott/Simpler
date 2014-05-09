#Simpler

At its core, Simpler is a philosophy on or a pattern for .NET class design. Simpler can help developers—especially teams of developers—build complex projects using consistent, readable classes that can be easily integrated with each other. 

Simpler’s primary goal is to help developers create quick, simple solutions while writing the least amount of code possible.

##Key Benefits of Simpler
- Eliminates the need to discuss and decide on class design 
- Makes the code more understandable, consistent, and readable
- Simplifies writing unit tests
- Provides a cleaner method of addressing cross-cutting concerns
- Simplifies maintenance by making it easier to find business logic 

##The Simpler Philosophy
In the traditional Object-oriented Programming (OOP) approach, classes define objects (named with nouns) and include data and business logic (methods named with verbs). 

![OOP Diagram](/images/OOPDiagram.png)

But when an application needs to interact with several classes, it isn’t always clear where that business logic should be stored. So “manager” or “service” classes are often created for all of the business logic affecting multiple object classes. 

With OOP, developers must constantly make decisions about where to place new business logic, a problem which is exacerbated by complex projects or when working with a team. Even with careful class design, it’s not always obvious where to find particular business logic, especially when maintaining code.

However, with Simpler, data and business logic are divided into small, discrete “building blocks.” Although you’ll have a lot of small classes—instead of a few, very large classes—these classes can easily integrate with each other without “manager” or “service” classes. 

Simpler also separates data from business logic, with data defined in Model classes and business logic in Task classes. 

![Simpler Diagram](/images/SimplerDiagram.png)

Model classes are typically just plain old CLR objects (POCOs) and only contain properties. But a Task *does things*; each Task class is the equivalent of a discrete action. Simpler provides functionality for Tasks.

##Installing Simpler

Use Nuget. Simpler works with .NET 3.5 and above.

##Using Simpler
Using Simpler is, well, *simple*.  

1. [Create a Task class] (#creating_tasks). 
2. [Instantiate the Task] (#instantiating_tasks) using the Task.New() method. 
3. Execute the task. 

Simpler also provides other functionality: 

- From within a Task, [execute other Tasks (sub-tasks)] (#injecting-sub-tasks)
- When testing a Task with sub-tasks, isolate the Task logic by [mocking the sub-tasks] (#mocking)
- [Perform actions before or after execute] (#eventsattribute), which is especially useful for addressing cross-cutting concerns such as logging

> add more in here about doing other things—stats and name

###<a name="creating_tasks"></a>Creating Tasks

When you create a Task, you should name it so that everyone can easily identify what the Task does. Follow these naming rules:
 
- Begin each Task name with a verb (because the Task is action, it’s doing something)
- Clearly state the Task’s purpose in the name

>**Example:** A Task that parses an XML file containing projects might be called *ParseProjectsXml*. But it shouldn’t be called *XmlProjectParser* or *XmlProject*.

Simpler provides 3 base classes for defining Tasks: 

- [`InTask`] (#intask) (applies business logic to an input but doesn’t produce any output)
- [`OutTask`] (#outtask) (produces output using business logic but does not accept any input)
- [`InOutTask`] (#inouttask) (applies business logic to an input and produces an output)

>**Note:** Simpler also includes a `Task` base class. This `Task` base class provides backwards compatibility for Simpler 1.0 and also includes the static `Task.New<TTask>()` method, which is a factory method for [instantiating tasks] (#instantiating_tasks). In addition, all Tasks inherit  the [`Name`] (#name) property
and the [`Stats`] (#stats) property from this base class.

####<a name="intask"></a>InTask
An `InTask` applies business logic to an input but doesn’t produce an output. For example, an `InTask` might receive an input of a filename and then add a prefix to the filename. 

For an `InTask`, you must enter a generic parameter type that defines the type of input. This input is exposed to the `Execute()` method through the `In` property.

To make the `In` property a container for all input, you can define an `Input` class inside the `InTask`. This `Input` class contains the input as properties and is passed to the `InTask` as the generic parameter type. 

```c#
public class AddPrefix : InTask<AddPrefix.Input>
{
    public class Input
    {
        public string FileName { get; set; }
        public string Directory { get; set; }
    }

    public override void Execute()
    {
        string directory = In.Directory;
        string fileName = In.FileName;
        string prefix = @"Obsolete_";

        File.Move(directory + fileName, directory + prefix + fileName);
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
                    SightingDate as ObservationDate,
                    Species as Species
                from
                    [Sight].[Birds]
                order by
                    ObservationDate
                ";
            Out.BirdSightings = Db.GetMany<BirdSighting>(connection, sql);
        }
    }
}
```

####InOutTask

An `InOutTask` is a combination of an [`InTask`] (#intask) and an [`OutTask`] (#outtask). An `InOutTask` applies business logic to an input and produces an output. For example, an `InOutTask` might have an input of a list and then process business logic to narrow the list to those entries meeting particular characteristics, and then output that narrowed list.
 
For an `InOutTask`, you must enter 2 generic parameter types: one for the type of input and one for the type of output. As with an `InTask` and an `OutTask`, these parameters are exposed in the `Execute()` method through the `In` and `Out` properties respectively. You can also define `Input` and `Output` classes inside the `InOutTask`. Refer to [InTask] (#intask) and [OutTask] (#outtask) for additional information. 

```c#
public class FetchFeaturedStaff : InOutTask<FetchFeaturedStaff.Input, FetchFeaturedStaff.Output>
{
    public class Input
    {
        public List<StaffModel> Staff { get; set; }
    }

    public class Output
    {
        public List<StaffModel> SelectedStaff { get; set; }
    }

    public override void Execute()
    {
        Out.SelectedStaff = new List<StaffModel>();

        foreach (StaffModel stf in In.Staff)
        {
            if (stf.Featured)
            {
                Out.SelectedStaff.Add(stf);
            }
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
        var prefix = Task.New<AddPrefix>();
        prefix.In.Directory = @"ReceivedFiles";
        prefix.Execute();
    }
}
```

###<a name="injecting-sub-tasks"></a>Injecting Sub-tasks

With Simpler, a Task contains the smallest piece of useable functionality. Therefore, you’ll often need a Task to execute other Tasks, referenced as sub-tasks, which creates a dependency between the Tasks. But to prevent tight coupling between the Tasks, Simpler provides automatic sub-task injection. 

>**Note:** Sub-task injection also supports advanced scenarios, such as injecting dependencies at runtime. This type of injection is typically used for testing purposes. 

To inject sub-tasks within a Task class, define the sub-tasks as properties. Any Task can be referenced as a sub-task. A sub-task is no different from a normal Task—it’s only called a sub-task when it’s defined as a property on another Task. 

Before executing a Task, Simpler checks whether the Task has any sub-tasks. If so, Simpler automatically creates the sub-tasks and injects them into the Task properties. 

```c#
{
    public class AddPrefix : InTask<AddPrefix.Input>
    {
        public class Input
        {
            public string Directory { get; set; }
        }

        public GetFiles GetFiles { get; set; }

        public override void Execute()
        {
            string prefix = @"Obsolete_";

            GetFiles.In.Directory = In.Directory;
            GetFiles.Execute();
            PrefixFile[] files = GetFiles.Out.Files;

            foreach (var file in files)
            {
                string fileName = file.FileName;
                string directory = file.FileDirectory

                File.Move(directory + fileName, directory + prefix + fileName);
            }
        }
    }
}
```

###Writing Tests

By design, Simpler forces you to create code that is easy to test. Each Task clearly defines its inputs, outputs, and the discrete code to test, so writing a test is typically straightforward. Simpler works particularly well in a [TDD workflow] (#tdd-example). 

>**Note:** If your Task includes sub-tasks, you can [mock the sub-task behavior] (#mocking) using `Fake.Task<TTask>()`, which isolates the logic of the Task being tested. 

```c#
[TestFixture]
public class FetchCertainStuffTest
{
    [Test]
    public void should_return_9_pocos()
    {
        // Arrange
        var task = Task.New<FetchCertainStuff>();
        task.In.SomeCriteria = "whatever";

        // Act
        task.Execute();

        // Assert
        Assert.That(task.Out.Stuff.Length, Is.EqualTo(9));
    }
}
```

####<a name="mocking"></a>Mocking

When writing a test for a Task with an [injected sub-task] (#injecting-sub-tasks), you may want to isolate the test to only the Task’s business logic. Using `Fake.Task<TTask>()`, you override the sub-task’s `Execute()` logic and instead define the data you want the test to “mock” being provided by the sub-task. 

If the Task has many sub-tasks, it’s easier to begin with a call to `Fake.SubTasks()` and then fake the individual tasks that are needed for the specific test scenario. 

#####Example

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

####<a name="tdd-example"></a>TDD Example Workflow

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

        Ask.In.Question = "Is this cool?";

        for (var i = 0; i < In.AnnoyanceLevel; i++)
        {
            Ask.Execute();
        }

        // "BeAnnoying finished." will be logged to the console after Execute() finishes.
    }
}
```

###Using Stats and Name Properties 

The `Stats` and `Name` properties, which all Tasks inherit from the `Task` base class, are typically used in test or logging. 

####<a name="stats"></a>Stats



####<a name="name"></a>Name

Returns the real name (without proxy)



##Acknowledgments

***consider what to do with this since it hasn’t been kept up to date***
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

##License
Simpler is licensed under the MIT License. A copy of the MIT license can be found in the LICENSE file.
