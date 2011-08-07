#Simpler

You probably won't like Simpler.  If you enjoy configuring ORMs, interfacing with IOC frameworks, generating code, and building complex domain models, then you will probably hate Simpler.  Simpler avoids all that code noise and takes a simpler approach, but you'll probably need an open mind to embrace it.

Simpler's primary goal is help developers create quick, simple solutions while writing the least amount of code possible.  Simpler also emphasizes the ability to discover code - a developer maintaining a code base shouldn't have to wade through a bunch of documentation, class hierarchies, code wrappers, and other clever abstractions, just to make a simple code change.  Every piece of code in an application should have a clearly visible business purpose - the rest is just noise.

"What is it?"

For the most part, Simpler is just a philosophy on .NET class design.  All classes that contain functionality are called tasks.  Each task will have a single responsibility, and its class name will clearly explain that responsibility using a verb (instead of a noun).  Since each task class only does one thing it only needs one method - Execute().  Simpler comes with a Task base class, a static TaskFactory class for instantiating tasks, along with various built-in tasks that you can use as sub-tasks in your tasks.

    todo - dynamic task code example

Scared by the dynamic Inputs and Outputs properties?  OK, fine, ignore the Inputs and Outputs properties, create POCOs for your task's input and outputs, and declare them as properties on your task.  This will give you some type checking at compile time, but it's more work and adds some noise.

    todo - static task code example

"What's the purpose of the TaskFactory?"

TaskFactory appears to just return an instance of the given task type, but actually it returns a proxy to the task.  The proxy enables Simpler to provide notifications before and after task execution, and when execution results in an unhanded exception.  You can respond to these notifications by implementing an ExecutionCallbackAttribute, and then applying that attribute to a task (or tasks).  For example, the built-in InjectSubTasks attribute will automatically instantiate sub-task properties (only if null) before task execution, and automatically dispose of them after execution.  This simplifies the code, but more importantly it provides for overriding sub-tasks at run-time (usually for testing purposes).

    todo - task with injectsubtask attribute code example

"What about testing?"

The usual approach to testing in .NET is to create a separate .Tests project where you put your test classes and methods.  This works fine - I did it this way for years.  However, this approach causes you to open two class files, from two different projects, in order to add new functionality.  Is that really necessary?  Why not put the tests right next to the code?

    todo - test example

[Explain how the testing works]  For TDD, implement the Tests() method before you implement the Execute().

"What about database access?"

Simpler provides a small set of tasks for interacting with IDbCommand.  Using SQL, Simpler makes it pretty easy to get data out of a database and into .NET objects, or persist data from .NET objects to a database.  Simpler isn't a full-featured ORM, but it gets the job done.

    todo - data access examples

"Sql?  I thought the trendy thing was to use a framework that writes the SQL for you?

Maybe, and there are probably cases where using full-featured ORM tool like nHibernate would be advantageous.  However, I recognize SQL as a very powerful DSL for interfacing with relational databases, and I prefer it to creating XML and/or fluent interface mappings.  And yes, I'm aware of the NoSql alternatives, and I'm intrigued.  However, I write enterprise software for a living and that means I'm interfacing with relational databases, and I don't expect that to change anytime soon.

In summary, Simpler is a tool for developing applications as sets of consistent, discrete, reusable classes that can easily be moved or adjusted as necessary.  Simpler works great in team environments because all developers on the team design classes using the same techniques and terminology, which results in a consistent code base.  Simpler makes it dead simple to add automated unit tests.  Simpler fits like a glove with a ASP.NET MVC (add a Tasks folder next to your Controllers, Models, and Views folders).  And finally, Simpler can be installed in seconds using NuGet.

**Simpler is licensed under the MIT License.  A copy of the MIT license can be found in the LICENSE file.**
