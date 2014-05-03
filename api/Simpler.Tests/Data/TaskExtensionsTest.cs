using NUnit.Framework;
using Simpler.Data;
using Simpler.Tests.Data.Tasks;

namespace Simpler.Tests.Data.Tasks
{
    public class SelectEverything : Task
    {
        public override void Execute() { }
    }
}

namespace Simpler.Tests.Data
{
    [TestFixture]
    public class TaskExtensionTest
    {
        [Test]
        public void should_find_sql_in_corresponding_sql_file()
        {
            var t = Task.New<SelectEverything>();
            Assert.That(t.Sql().Contains("select * from everything"));
        }
    }
}