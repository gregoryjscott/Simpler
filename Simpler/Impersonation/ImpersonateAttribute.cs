using Simpler.Construction;
using System;

namespace Simpler.Impersonation
{
    public class ImpersonateAttribute : ExecutionCallbacksAttribute
    {
        public ImpersonateAttribute(dynamic userToImpersonate)
        {
            UserName = userToImpersonate.UserName;
            Password = userToImpersonate.Password;
            Domain = userToImpersonate.Domain;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }

        public override void BeforeExecute(Task taskBeingExecuted)
        {
        }

        public override void AfterExecute(Task taskBeingExecuted)
        {
        }

        public override void OnError(Task taskBeingExecuted, Exception exception) { }
    }
}
