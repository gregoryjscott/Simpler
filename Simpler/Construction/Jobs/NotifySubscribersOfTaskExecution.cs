using System;
using Castle.DynamicProxy;

namespace Simpler.Construction.Jobs
{
    /// <summary>
    /// Job that notifies all subscribers to job execution.  Subscriptions are made by decorating the
    /// job with an attribute that is a subclass of ExecutionCallbacksAttribute or ExecutionOverrideAttribute.
    /// </summary>
    public class NotifySubscribersOfJobExecution : Job
    {
        // Inputs
        public virtual Job ExecutingJob { get; set; }
        public virtual IInvocation Invocation { get; set; }

        public override void Execute()
        {
            var callbackAttributes = Attribute.GetCustomAttributes(ExecutingJob.GetType(), typeof (ExecutionCallbacksAttribute));
            var overrideAttribute = Attribute.GetCustomAttribute(ExecutingJob.GetType(), typeof(ExecutionOverrideAttribute));

            // Send BeforeExecute notifications.
            foreach (var callbackAttribute in callbackAttributes)
            {
                ((ExecutionCallbacksAttribute)callbackAttribute).BeforeExecute(ExecutingJob);
            }

            // Execute, and temporarily catch any exceptions so notifications can be sent.
            try
            {
                // If the job execution has been overridden, simply the pass on the job, otherwise proceed with the invocation.
                if (overrideAttribute != null)
                {
                    ((ExecutionOverrideAttribute)overrideAttribute).ExecutionOverride(Invocation);
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
                    ((ExecutionCallbacksAttribute) callbackAttributes[i]).OnError(ExecutingJob, exception);
                }

                throw;
            }
            finally
            {
                // Send the AfterExecute notifications.
                for (var i = callbackAttributes.Length - 1; i >= 0; i--)
                {
                    ((ExecutionCallbacksAttribute) callbackAttributes[i]).AfterExecute(ExecutingJob);
                }
            }
        }
    }
}