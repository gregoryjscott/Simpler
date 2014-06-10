using NUnit.Framework;
using Simpler;
using Simpler.Mocks;

namespace Simpler.Core.Tasks
{
    [TestFixture]
    public class ProxyTests
    {
        [Test]
        public void should_create_proxy_type()
        {
            var createProxyType = Task.New<CreateProxyType>();
            createProxyType.In.Type = typeof(MockTask);
            createProxyType.Execute();
            var proxyType = createProxyType.Out.TypeBuilder.CreateType();

            var expected = typeof(MockTask).FullName.Replace("MockTask", "MockTaskProxy");
            Assert.That(proxyType.FullName, Is.EqualTo(expected));
        }
    }
}