using NUnit.Framework;
using Simpler;
using Simpler.Mocks;
using System.Reflection.Emit;
using System;

namespace Simpler.Core.Tasks
{
    [TestFixture]
    public class ProxyTests
    {
        [Test]
        public void should_create_proxy_type()
        {
            var proxyType = CreateProxyTypeBuilder().CreateType();

            var expected = typeof(MockTask).FullName.Replace("MockTask", "MockTaskProxy");
            Assert.That(proxyType.FullName, Is.EqualTo(expected));
        }

        [Test]
        public void should_move_base_execute()
        {
            var proxyTypeBuilder = CreateProxyTypeBuilder();
            var baseType = typeof(MockTask);

            var moveBaseExecute = Task.New<MoveBaseExecute>();
            moveBaseExecute.In.TypeBuilder = proxyTypeBuilder;
            moveBaseExecute.In.BaseType = baseType;
            moveBaseExecute.Execute();

            var proxyType = proxyTypeBuilder.CreateType();
            var proxy = (MockTask)Activator.CreateInstance(proxyType);

            proxyType.GetMethod("BaseExecute").Invoke(proxy, null);
            Assert.That(proxy.WasExecuted, Is.True);
        }

        static TypeBuilder CreateProxyTypeBuilder()
        {
            var createProxyType = Task.New<CreateProxyType>();
            createProxyType.In.Type = typeof(MockTask);
            createProxyType.Execute();
            return createProxyType.Out.TypeBuilder;
        }
    }
}