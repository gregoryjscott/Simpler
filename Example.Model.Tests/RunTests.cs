using NUnit.Framework;
using Simpler;

namespace Example.Model.Tests
{
    [TestFixture]
    public class RunTests
    {
        [Test]
        public void ExampleModel() { Test.Assembly("Example.Model"); }
    }
}
