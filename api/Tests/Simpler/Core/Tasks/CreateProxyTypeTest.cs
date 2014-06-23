using NUnit.Framework;
using Simpler;
using Simpler.Mocks;
using System.Reflection.Emit;
using System;

namespace Simpler.Core.Tasks
{
    [TestFixture]
    public class CreateProxyTypeTest
    {
        [Test]
        public void should_create_proxy_type()
        {
            var proxyType = CreateProxyTypeBuilder().CreateType();

            var expected = typeof(MockTask).FullName.Replace("MockTask", "MockTaskProxy");
            Assert.That(proxyType.FullName, Is.EqualTo(expected));
        }

        public static TypeBuilder CreateProxyTypeBuilder()
        {
            var createProxyType = Task.New<CreateProxyType>();
            createProxyType.In.Type = typeof(MockTask);
            createProxyType.Execute();
            return createProxyType.Out.TypeBuilder;
        }
    }
}
