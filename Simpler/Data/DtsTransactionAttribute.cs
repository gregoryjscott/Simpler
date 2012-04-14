using System;
using System.Transactions;
using Simpler.Construction;

namespace Simpler.Data
{
    // todo - this needs to be tested (then made public)

    class DtsTransactionAttribute : ExecutionCallbacksAttribute
    {
        public DtsTransactionAttribute(TimeSpan? timeout = null)
        {
            // Default to 30 seconds if a timeout is not supplied.
            if (timeout == null)
            {
                _timeout = TimeSpan.FromSeconds(30);
            }
        }

        readonly TimeSpan _timeout;
        TransactionScope _transactionScope;
        bool _errorOccurred;

        public override void BeforeExecute(Job jobBeingExecuted)
        {
            // Reset the flag.
            _errorOccurred = false;

            if (_transactionScope == null)
            {
                _transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { Timeout = _timeout });
            }
        }

        public override void AfterExecute(Job jobBeingExecuted)
        {
            if (!_errorOccurred)
            {
                _transactionScope.Complete();
            }

            _transactionScope.Dispose();
            _transactionScope = null;
        }

        public override void OnError(Job jobBeingExecuted, Exception exception)
        {
            // Set the flag so that the transaction will not be committed.
            _errorOccurred = true;
        }
    }
}
