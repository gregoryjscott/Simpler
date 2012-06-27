using System;
using System.Data;

namespace Simpler.Mocks
{
    class MockConnection : IDbConnection
    {
        public bool CreateCommandWasCalled { get; set; }
        public bool ConnectionWasOpened { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public IDbCommand CreateCommand()
        {
            CreateCommandWasCalled = true;
            return new MockCommand();
        }

        public void Open()
        {
            ConnectionWasOpened = true;
        }

        public string ConnectionString
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ConnectionTimeout
        {
            get { throw new NotImplementedException(); }
        }

        public string Database
        {
            get { throw new NotImplementedException(); }
        }

        public ConnectionState State
        {
            get { return ConnectionState.Closed; }
        }
    }
}
