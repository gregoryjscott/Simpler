using Simpler;
using System;

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
        Console.WriteLine(String.Format("{0} bombed! Details: {1}.", task.Name, exception));
    }
}


