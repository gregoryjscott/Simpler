using System;
using System.Transactions;

namespace MvcExample.Tests
{
    public static class RollbackTransaction
    {
        public static IDisposable Create()
        {
            return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions());
        }
    }
}
