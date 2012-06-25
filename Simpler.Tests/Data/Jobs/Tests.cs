using NUnit.Framework;
using Simpler.Data.Jobs;

namespace Simpler.Tests.Data.Jobs
{
    [TestFixture]
    public class Tests
    {
        [Test] 
        public void Simpler() { Describe.Assembly("Simpler"); }

        [Test] 
        public void BuildObject() { Describe.Job<BuildObject<object>>(); }
    }
}
