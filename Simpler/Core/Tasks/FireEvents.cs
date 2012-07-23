using System;
using Castle.DynamicProxy;

namespace Simpler.Core.Jobs
{
    public class FireEvents : Task
    {
        // Inputs
        public virtual Task Task { get; set; }
        public virtual IInvocation Invocation { get; set; }

        public override void Run()
        {
            var callbackAttributes = Attribute.GetCustomAttributes(Task.GetType(), typeof (EventsAttribute));
            var overrideAttribute = Attribute.GetCustomAttribute(Task.GetType(), typeof (OverrideAttribute));

            try
            {
                foreach (var callbackAttribute in callbackAttributes)
                {
                    ((EventsAttribute)callbackAttribute).BeforeRun(Task);
                }

                if (overrideAttribute != null)
                {
                    ((OverrideAttribute)overrideAttribute).RunOverride(Invocation);
                }
                else
                {
                    Invocation.Proceed();
                }
            }
            catch (Exception exception)
            {
                for (var i = callbackAttributes.Length - 1; i >= 0; i--)
                {
                    ((EventsAttribute) callbackAttributes[i]).OnError(Task, exception);
                }

                throw;
            }
            finally
            {
                for (var i = callbackAttributes.Length - 1; i >= 0; i--)
                {
                    ((EventsAttribute) callbackAttributes[i]).AfterRun(Task);
                }
            }
        }
    }
}