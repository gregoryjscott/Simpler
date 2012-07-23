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

namespace Example.Model.Jobs
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void FetchPlayer() { Describe.Job<FetchPlayer>(); }

        [Test]
        public void FetchPlayers() { Describe.Job<FetchPlayers>(); }

        [Test]
        public void UpdatePlayer() { Describe.Job<UpdatePlayer>(); }
    }
}
