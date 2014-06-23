using NUnit.Framework;
using Simpler;
using Simpler.Mocks;
using System.Reflection.Emit;
using System;

namespace Simpler.Core.Tasks
{
    [TestFixture]
    public class BuildProxyExecuteTest
    {
        [Test]
        public void should_build_proxy_execute()
        {
            var proxyTypeBuilder = CreateProxyTypeTest.CreateProxyTypeBuilder();

            SaveBaseExecuteTest.SaveBaseExecute(proxyTypeBuilder);

            var buildProxyExecute = new BuildProxyExecute();
            buildProxyExecute.In.TypeBuilder = proxyTypeBuilder;
            buildProxyExecute.Execute();

            var proxyType = proxyTypeBuilder.CreateType();
            var proxy = (MockTask)Activator.CreateInstance(proxyType);

            proxyType.GetMethod("Execute").Invoke(proxy, null);
            Assert.That(proxy.WasExecuted, Is.True);
        }
    }
}