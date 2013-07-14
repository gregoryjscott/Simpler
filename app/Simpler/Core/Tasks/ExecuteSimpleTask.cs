using System;
using Castle.DynamicProxy;

namespace Simpler.Core.Tasks
{
    public class ExecuteSimpleTask : InSimpleTask<ExecuteSimpleTask.Input>
    {
        public class Input
        {
            public SimpleTask SimpleTask { get; set; }
            public IInvocation Invocation { get; set; }
        }

        public override void Execute()
        {
            var callbackAttributes = Attribute.GetCustomAttributes(In.SimpleTask.GetType(), typeof (EventsAttribute));
            var overrideAttribute = Attribute.GetCustomAttribute(In.SimpleTask.GetType(), typeof (OverrideAttribute));

            var beforeTime = DateTime.Now;
            try
            {
                foreach (var callbackAttribute in callbackAttributes)
                {
                    ((EventsAttribute)callbackAttribute).BeforeExecute(In.SimpleTask);
                }

                if (overrideAttribute != null)
                {
                    ((OverrideAttribute)overrideAttribute).ExecuteOverride(In.Invocation);
                }
                else
                {
                    In.Invocation.Proceed();
                }
            }
            catch (Exception exception)
            {
                for (var i = callbackAttributes.Length - 1; i >= 0; i--)
                {
                    ((EventsAttribute) callbackAttributes[i]).OnError(In.SimpleTask, exception);
                }

                throw;
            }
            finally
            {
                for (var i = callbackAttributes.Length - 1; i >= 0; i--)
                {
                    ((EventsAttribute) callbackAttributes[i]).AfterExecute(In.SimpleTask);
                }

                var afterTime = DateTime.Now;
                var duration = afterTime - beforeTime;
                In.SimpleTask.Stats.ExecuteDurations.Add(duration);
            }
        }
    }
}