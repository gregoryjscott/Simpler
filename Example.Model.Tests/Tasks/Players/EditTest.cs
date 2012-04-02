using Example.Model.Tasks.Players;
using NUnit.Framework;
using Simpler;

namespace Example.Model.Tests.Tasks.Players
{
    [TestFixture]
    public class EditTest
    {
        [SetUp]
        public void SetUp()
        {
            Config.SetDataDirectory();
        }

        [Test]
        public void should_return_a_player_for_editing()
        {
            Test<Edit>.Create()
                .Arrange(t => t.Set(new Edit.In {PlayerId = 1}))
                .Act()
                .Assert(t => Assert.That(t.Output.Player.PlayerId, Is.EqualTo(1)));
        }
    }
}
