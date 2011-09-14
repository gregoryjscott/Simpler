using System;
using System.Transactions;
using Simpler.Construction;

namespace Simpler.Data
{
    // todo - this needs to be tested (then made public)

    class TransactionAttribute : ExecutionCallbacksAttribute
    {
        public TransactionAttribute(TimeSpan? timeout = null)
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

        public override void BeforeExecute(Task taskBeingExecuted)
        {
            // Reset the flag.
            _errorOccurred = false;

            if (_transactionScope == null)
            {
                _transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { Timeout = _timeout });
            }
        }

        public override void AfterExecute(Task taskBeingExecuted)
        {
            if (!_errorOccurred)
            {
                _transactionScope.Complete();
            }

            _transactionScope.Dispose();
            _transactionScope = null;
        }

        public override void OnError(Task taskBeingExecuted, Exception exception)
        {
            // Set the flag so that the transaction will not be committed.
            _errorOccurred = true;
        }
    }
}
