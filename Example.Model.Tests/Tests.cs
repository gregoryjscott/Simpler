using Example.Model.Tasks;
using NUnit.Framework;
using Simpler;

namespace Example.Model
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Assembly() { Describe.Assembly("Example.Model"); }
    }
}

namespace Example.Model.Tasks
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void FetchPlayer() { Describe.Task<FetchPlayer>(); }

        [Test]
        public void FetchPlayers() { Describe.Task<FetchPlayers>(); }

        [Test]
        public void UpdatePlayer() { Describe.Task<UpdatePlayer>(); }
    }
}
