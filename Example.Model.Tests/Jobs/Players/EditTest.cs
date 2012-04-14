using Example.Model.Jobs.Players;
using NUnit.Framework;
using Simpler;

namespace Example.Model.Tests.Jobs.Players
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
            Test<Edit>.New()
                .Arrange(job => job.Set(new Edit.In {PlayerId = 1}))
                .Act()
                .Assert(job => Assert.That(job._Out.Player.PlayerId, Is.EqualTo(1)));
        }
    }
}
