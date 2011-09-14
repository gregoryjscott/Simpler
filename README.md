#Simpler

You probably won't like Simpler.  If you enjoy spending your time configuring ORMs, interfacing with DI/IOC frameworks, generating code, and building complex domain models, then you will probably hate Simpler.  Simpler's primary goal is help developers create quick, simple solutions while writing the least amount of code possible.  Every piece of code in an application should have a clearly visible business purpose - the rest is just noise.


###"What is it?"

For the most part, Simpler is just a philosophy on .NET class design.  All classes that contain functionality are defined as Tasks.  Each Task will have a single responsibility, and its class name will clearly explain that responsibility (using a verb instead of a noun).  Since each Task class only does one thing it only needs one method - Execute().  Simpler comes with a Task base class, a static TaskFactory class for instantiating Tasks, along with various built-in Tasks that you can use as sub-tasks in your Tasks.

    todo - dynamic task code example

Scared by the dynamic Inputs and Outputs properties?  Fine - ignore the Inputs and Outputs properties, create POCOs for your Task's input and outputs, and declare them as properties on your Task.

    todo - static task code example


###"What's the purpose of the TaskFactory?"

TaskFactory appears to just return an instance of the given Task type, but it actually returns a proxy to the Task.  The proxy allows for intercepting Task Execute() calls and performing actions before and/or after the Task execution.  For example, the built-in InjectSubTasks attribute will automatically instantiate sub-task properties (only if null) before Task execution, and automatically dispose of them after execution.

    todo - task with injectsubtask attribute code example

This simplifies the code, but more importantly it allows for mocking sub-tasks for testing purposes.


###"What about database access?"

Simpler provides a small set of Tasks for interacting with IDbCommand.  Using SQL, Simpler makes it pretty easy to get data out of a database and into .NET objects, or persist data from .NET objects to a database.

    todo - data access examples

Simpler isn't a full-featured ORM, but it gets the job done.


In summary, Simpler is a tool for developing applications as sets of consistent, discrete, reusable Task classes that can easily be moved around or adjusted as necessary.  Simpler works great in team environments because all developers on the team design classes using the same techniques and terminology, resulting in a consistent code base.  Simpler fits like a glove with a ASP.NET MVC (add a Tasks folder next to your Controllers, Models, and Views folders).  And finally, Simpler can be added to your .NET project in seconds using NuGet.

**Simpler is licensed under the MIT License.  A copy of the MIT license can be found in the LICENSE file.**
