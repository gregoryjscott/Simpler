using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleTask.Attributes;

namespace SimpleTask.Tests.Mocks
{
    [SubTaskInjection(Enabled = false)]
    public class MockTaskWithSubTaskInjectionDisabled : Task
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
