using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.DynamicProxy;

namespace Simpler.Core
{
    public class FakeInvocation<TTask> : IInvocation where TTask : T
    {
        public FakeInvocation(T task, Action<TTask> execute)
        {
            _task = task;
            _execute = execute;
        }

        readonly T _task;
        readonly Action<TTask> _execute;

        public void Proceed()
        {
            _execute((TTask)_task);
        }

        #region Not Implemented

        public void SetArgumentValue(int index, object value)
        {
            throw new NotImplementedException();
        }

        public object GetArgumentValue(int index)
        {
            throw new NotImplementedException();
        }

        public MethodInfo GetConcreteMethod()
        {
            throw new NotImplementedException();
        }

        public MethodInfo GetConcreteMethodInvocationTarget()
        {
            throw new NotImplementedException();
        }

        public object Proxy
        {
            get { throw new NotImplementedException(); }
        }

        public object InvocationTarget
        {
            get { throw new NotImplementedException(); }
        }

        public Type TargetType
        {
            get { throw new NotImplementedException(); }
        }

        public object[] Arguments
        {
            get { throw new NotImplementedException(); }
        }

        public Type[] GenericArguments
        {
            get { throw new NotImplementedException(); }
        }

        public MethodInfo Method
        {
            get { throw new NotImplementedException(); }
        }

        public MethodInfo MethodInvocationTarget
        {
            get { throw new NotImplementedException(); }
        }

        public object ReturnValue
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion
    }
}
