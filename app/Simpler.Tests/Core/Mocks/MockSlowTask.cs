using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Simpler.Tests.Core.Mocks
{
    public class MockSlowTask : Task
    {
        public override void Execute()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(50);
            }
        }
    }
}
