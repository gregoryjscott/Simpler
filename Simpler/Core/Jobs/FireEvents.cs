using System;
using Castle.DynamicProxy;

namespace Simpler.Core.Jobs
{
    public class FireEvents : Job
    {
        // Inputs
        public virtual Job Job { get; set; }
        public virtual IInvocation Invocation { get; set; }

        public override void Run()
        {
            var callbackAttributes = Attribute.GetCustomAttributes(Job.GetType(), typeof (EventsAttribute));
            var overrideAttribute = Attribute.GetCustomAttribute(Job.GetType(), typeof (OverrideAttribute));

            try
            {
                foreach (var callbackAttribute in callbackAttributes)
                {
                    ((EventsAttribute)callbackAttribute).BeforeRun(Job);
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
                    ((EventsAttribute) callbackAttributes[i]).OnError(Job, exception);
                }

                throw;
            }
            finally
            {
                for (var i = callbackAttributes.Length - 1; i >= 0; i--)
                {
                    ((EventsAttribute) callbackAttributes[i]).AfterRun(Job);
                }
            }
        }
    }
}