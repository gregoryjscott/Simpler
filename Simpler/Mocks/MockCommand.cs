using System;
using System.Data;

namespace Simpler.Mocks
{
    class MockCommand : IDbCommand
    {
        public void Dispose()
        {
        }

        public void Prepare()
        {
            throw new NotImplementedException();
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public IDbDataParameter CreateParameter()
        {
            throw new NotImplementedException();
        }

        public int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        public IDataReader ExecuteReader()
        {
            throw new NotImplementedException();
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        public IDbConnection Connection { get; set; }

        public IDbTransaction Transaction
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string CommandText { get; set; }

        public int CommandTimeout
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public CommandType CommandType
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IDataParameterCollection Parameters
        {
            get { throw new NotImplementedException(); }
        }

        public UpdateRowSource UpdatedRowSource
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}
