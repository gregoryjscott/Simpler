using NUnit.Framework;
using Simpler;
using Simpler.Mocks;
using System.Reflection.Emit;
using System;

namespace Simpler.Core.Tasks
{
    [TestFixture]
    public class BuildExecuteOverrideTest
    {
        [Test]
        public void should_build_execute_override()
        {
            var proxyTypeBuilder = CreateProxyTypeTest.CreateProxyTypeBuilder();

            var overrideWorked = false;
            Action<Task> action = t => { overrideWorked = true; };

            var buildExecuteOverride = Task.New<BuildExecuteOverride>();
            buildExecuteOverride.In.TypeBuilder = proxyTypeBuilder;
            buildExecuteOverride.Execute();

            var proxyType = proxyTypeBuilder.CreateType();
            var proxy = (MockTask)Activator.CreateInstance(proxyType, action);
            proxyType.GetMethod("Execute").Invoke(proxy, null);
            Assert.That(overrideWorked, Is.True);
        }
    }
}
