using NUnit.Framework;
using Simpler.Data;
using Simpler.Tests.Data.Tasks;

namespace Simpler.Tests.Data
{
    [TestFixture]
    public class TaskExtensionTest
    {
        [Test]
        public void should_find_sql_in_corresponding_sql_file()
        {
            var selectEverything = Execute.Now<SelectEverything>();
            Assert.That(selectEverything.Sql().Contains("select * from everything"));
        }
    }
}

namespace Simpler.Tests.Data.Tasks
{
    public class SelectEverything : Task
    {
        public override void Execute() { }
    }
}
