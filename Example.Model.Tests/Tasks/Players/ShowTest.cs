using Example.Model.Tasks.Players;
using NUnit.Framework;
using Simpler;

namespace Example.Model.Tests.Tasks.Players
{
    [TestFixture]
    public class ShowTest
    {
        [SetUp]
        public void SetUp()
        {
            Config.SetDataDirectory();
        }

        [Test]
        public void should_return_a_player()
        {
            Test<Show>.Create()
                .Arrange(t => t.Set(new Show.In {PlayerId = 1}))
                .Act()
                .Assert(t => Assert.That(t.Output.Player.PlayerId, Is.GreaterThan(0)));
        }
    }
}
