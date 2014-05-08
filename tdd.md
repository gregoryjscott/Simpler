Figure out what you need to do, what inputs you'll need to supply, and the exact output you expect.

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

Write a test.

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

Verify the test fails.

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

Implement the business logic.

``` c#
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

Verify the test passes.

```
....................................................
Tests run: 52, Errors: 0, Failures: 0, Inconclusive: 0, Time: 5.237 seconds
  Not run: 0, Invalid: 0, Ignored: 0, Skipped: 0

```