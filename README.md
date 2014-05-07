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
> add an intro sentence. Also include links on the various items into the detail sections.

1. Create a Task class. 
2. Instantiate the Task using the Task.New() method. 

> add more in here about doing other things

##Creating Tasks

Simpler has 3 base classes for defining Tasks: 

- `InTask` (applies business logic to an input but doesn’t produce any output)
- `OutTask` (produces output using business logic but does not accept any input)
- `InOutTask` (applies business logic to an input and produces an output)

>Note:	Simpler also includes a `Task` base class, which is used for backwards compatibility for Simpler 1.0. In addition, all Tasks inherit the following from this base class: <need x-refs>

>- `Name` property
- `Stats` property
- Static `Task.New<TTask>()` method, which is a factory method for instantiating tasks 

###InTask
An `InTask` applies business logic to an input but doesn’t produce an output. For example, an `InTask` might receive an input of a file directory and then add a prefix to every filename in the directory. 

For an `InTask`, you must enter a generic parameter type that defines the type of input. This input is exposed to the `Execute()` method through the `In` property.

To make the `In` property a container for all input, you can define an `Input` class inside the `InTask`. This `Input` class contains the input as properties and is passed to the `InTask` as the generic parameter type. 

```c#
public class AddPrefix : InTask<AddPrefix.Input>
{
    public class Input
    {
        public string Directory { get; set; }
    }

    public override void Execute()
    {
        need code in here that adds a prefix to each file in the directory. Would be good if it used the In property.

    }
}
```

###OutTask

An `OutTask` has no input, but it uses business logic to produce an output. For example, an `OutTask` might query for a set of data and make the results available. 

For an `OutTask`, you must enter a generic parameter type that defines the type of output. This output is exposed in the `Execute()` method through the `Out` property. 

To make the `Out` property a container for all output, you can define an `Output` class inside the `OutTask`. This `Output` class contains the output as properties and is passed to the `OutTask` as the generic parameter type. 

>Note: The following example also uses Simpler.Data. 

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

###InOutTask

An `InOutTask` is a combination of an `InTask` and an `OutTask`. An `InOutTask` applies business logic to an input and produces an output. For example, an `InOutTask` might have an input of a list and then process business logic to narrow the list to those entries meeting particular characteristics, and then output that narrowed list.
 
For an `InOutTask`, you must enter 2 generic parameter types: one for the type of input and one for the type of output. As with an `InTask` and an `OutTask`, these parameters are exposed in the `Execute()` method through the `In` and `Out` properties respectively. You can also define `Input` and `Output` classes inside the `InOutTask`. Refer to InTask and OutTask for additional information. 

>Add links on InTask and OutTask in the last sentence.

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

###Naming Tasks

To enable everyone to easily identify what a Task accomplishes, follow these naming rules:
 
- Begin each Task name with a verb (because the Task is action, it’s doing something)
- Clearly state the Task’s purpose in the name

For example, a Task that parses an XML file containing projects might be called *ParseProjectsXML*. But it shouldn’t be called *XMLProjectParser* or *XMLProject*.

##Instantiating Tasks


##Injecting Sub-tasks


##Writing Test Cases


###Faking


###Stats


###Name



##Acknowledgments

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
