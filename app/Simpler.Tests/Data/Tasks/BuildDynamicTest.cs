using NUnit.Framework;
using Simpler.Data.Tasks;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class BuildDynamicTest
    {
        [Test]
        public void should_populate_dynamic_object_using_all_columns_in_the_data_record()
        {
            var buildDynamic = Execute.Now<BuildDynamic>(bd => {
                bd.In.DataRecord = ResultsTest.SetupJohn();
            });

            Assert.That(buildDynamic.Out.Object.Name, Is.EqualTo("John Doe"));
            Assert.That(buildDynamic.Out.Object.Age, Is.EqualTo(21));
        }
    }
}
