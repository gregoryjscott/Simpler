using Simpler;
using System;

namespace Examples
{
    public class LogAttribute : EventsAttribute
    {
        public override void BeforeExecute(Task task)
        {
            if (Enabled) Console.WriteLine("{0} started.", task.Name);
        }

        public override void AfterExecute(Task task)
        {
            if (Enabled) Console.WriteLine("{0} finished.", task.Name);
        }

        public override void OnError(Task task, Exception exception)
        {
            if (Enabled) Console.WriteLine("{0} bombed! Details: {1}.", task.Name, exception);
        }

        public static bool Enabled = true;
    }
}
