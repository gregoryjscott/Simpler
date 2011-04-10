Simpler's goal is to make developing quality .NET code simpler.  It dictates consistent class design, includes a micro-ORM, and enables enough dependency injection to do proper unit testing.  We like to use NuGet to add Simpler to our projects.

Simpler's philosophy to class design is that business concepts of an application should be represented using two types of classes - models and tasks.  Models represent application data and can be just POCOs, but they should only be used as data containers and therefore only contain properties.

    todo - model code example

Tasks contain the application functionality and must adhere to the single responsibility principle.  A task should have a meaningful name, and contain one Execute() method along with a set of input and/or output properties (usually models).

    todo - task code example

Tasks are instantiated by the TaskFactory.

    todo - taskfactory code example

TaskFactory appears to just return an instance of the given task type, but actually it returns a proxy to the task.  Simpler uses this proxy to provide hooks to task events - before execution, after execution, and on error.  We like to use these events to address cross-cutting concerns, such as logging and dependency injection, by simply decorating the task with an attribute.

    todo - task with injectsubtask attribute code example

Simpler includes a set of data tasks that collectively could be considered a micro-ORM.  Mappings are defined in SQL - we like SQL and we're good at it.  We like to put the SQL right in the task so that it's all in one place.

    todo - data task code example

We like all tasks to have a corresponding test class that contains the task's expectations/tests.  Simpler's sub-task injection (using InjectSubTasksAttribute) allows for mocking a task's dependencies (aka it's sub-tasks) in unit tests.  We like to use nUnit and Moq.

    todo - test with mocking a subtask code example 

Simpler allows for developing applications as sets of consistent, discrete, reusable classes that can easily be moved or adjusted as necessary (aka agile).  Simpler works great in team environments since everyone is designing classes using the same techniques and terminology, which results in a consistent code base.  Simpler makes it easy to write automated unit tests for you code.  Simpler fits really well with ASP.NET MVC.  We like Simpler.

***

**Simpler is licensed under the MIT License.  A copy of the MIT license can be found in the LICENSE file.**