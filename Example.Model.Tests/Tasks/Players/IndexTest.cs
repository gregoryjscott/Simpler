using Example.Model.Jobs.Players;
using NUnit.Framework;
using Simpler;

namespace Example.Model.Tests.Jobs.Players
{
    [TestFixture]
    public class IndexTest
    {
        [SetUp]
        public void SetUp()
        {
            Config.SetDataDirectory();
        }

        [Test]
        public void should_return_list_of_players()
        {
            Test<Index>.Create()
                .Act()
                .Assert(t => Assert.That(t.Output.Players.Length, Is.GreaterThan(0)));
        }
    }
}
