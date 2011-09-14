using Simpler.Construction;
using System;

namespace Simpler.Impersonation
{
    // todo - this needs to be tested (then made public)

    class ImpersonateAttribute : ExecutionCallbacksAttribute
    {
        public ImpersonateAttribute(dynamic userToImpersonate)
        {
            UserName = userToImpersonate.UserName;
            Password = userToImpersonate.Password;
            Domain = userToImpersonate.Domain;
        }

        ImpersonationContext _impersonationContext;

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }

        public override void BeforeExecute(Task taskBeingExecuted)
        {
            if (_impersonationContext == null)
            {
                _impersonationContext = new ImpersonationContext(UserName, Password, Domain);
            }
        }

        public override void AfterExecute(Task taskBeingExecuted)
        {
            _impersonationContext.Dispose();
            _impersonationContext = null;
        }

        public override void OnError(Task taskBeingExecuted, Exception exception) { }
    }
}
