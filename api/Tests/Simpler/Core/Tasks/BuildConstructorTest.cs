using NUnit.Framework;
using Simpler;
using Simpler.Mocks;
using System.Reflection.Emit;
using System;

namespace Simpler.Core.Tasks
{
    [TestFixture]
    public class BuildConstructorTest
    {
        [Test]
        public void should_build_constructor()
        {
            var proxyTypeBuilder = CreateProxyTypeTest.CreateProxyTypeBuilder();
            var fieldBuilder = CreateActionFieldTest.CreateActionField(proxyTypeBuilder);

            var buildConstructor = Task.New<BuildConstructor>();
            buildConstructor.In.TypeBuilder = proxyTypeBuilder;
            buildConstructor.In.ActionFieldInfo = fieldBuilder;
            buildConstructor.Execute();

            var proxyType = proxyTypeBuilder.CreateType();
            Action<Task> action = t => { };
            var proxy = (MockTask)Activator.CreateInstance(proxyType, action);

            Assert.That(proxy, Is.InstanceOf<MockTask>());
        }
    }
}
