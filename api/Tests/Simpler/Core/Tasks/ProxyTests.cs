using NUnit.Framework;
using Simpler;
using Simpler.Mocks;
using System.Reflection.Emit;
using System;
using System.Reflection;

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
        public void should_build_proxy_execute()
        {
            var proxyTypeBuilder = CreateProxyTypeBuilder();

            MoveBaseExecute(proxyTypeBuilder);

            var buildProxyExecute = new BuildProxyExecute();
            buildProxyExecute.In.TypeBuilder = proxyTypeBuilder;
            buildProxyExecute.Execute();

            var proxyType = proxyTypeBuilder.CreateType();
            var proxy = (MockTask)Activator.CreateInstance(proxyType);

            proxyType.GetMethod("Execute").Invoke(proxy, null);
            Assert.That(proxy.WasExecuted, Is.True);
        }

        [Test]
        public void should_build_execute_override()
        {
            var proxyTypeBuilder = CreateProxyTypeBuilder();

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

        [Test]
        public void should_move_base_execute()
        {
            var proxyTypeBuilder = CreateProxyTypeBuilder();

            MoveBaseExecute(proxyTypeBuilder);

            var proxyType = proxyTypeBuilder.CreateType();
            var proxy = (MockTask)Activator.CreateInstance(proxyType);

            proxyType.GetMethod("BaseExecute").Invoke(proxy, null);
            Assert.That(proxy.WasExecuted, Is.True);
        }

        [Test]
        public void should_create_action_field()
        {
            var proxyTypeBuilder = CreateProxyTypeBuilder();
            CreateActionField(proxyTypeBuilder);

            var proxyType = proxyTypeBuilder.CreateType();

            var actionField = proxyType.GetField("ExecuteAction");
            Assert.That(
                actionField.FieldType.FullName,
                Contains.Substring("Action`1").And.ContainsSubstring("Simpler.Task")
            );
        }

        [Test]
        public void should_build_constructor()
        {
            var proxyTypeBuilder = CreateProxyTypeBuilder();
            var fieldBuilder = CreateActionField(proxyTypeBuilder);

            var buildConstructor = Task.New<BuildConstructor>();
            buildConstructor.In.TypeBuilder = proxyTypeBuilder;
            buildConstructor.In.ActionFieldInfo = fieldBuilder;
            buildConstructor.Execute();

            var proxyType = proxyTypeBuilder.CreateType();
            Action<Task> action = t => { };
            var proxy = (MockTask)Activator.CreateInstance(proxyType, action);

            Assert.That(proxy, Is.InstanceOf<MockTask>());
        }

        static TypeBuilder CreateProxyTypeBuilder()
        {
            var createProxyType = Task.New<CreateProxyType>();
            createProxyType.In.Type = typeof(MockTask);
            createProxyType.Execute();
            return createProxyType.Out.TypeBuilder;
        }

        static void MoveBaseExecute(TypeBuilder typeBuilder)
        {
            var moveBaseExecute = Task.New<SaveBaseExecute>();
            moveBaseExecute.In.TypeBuilder = typeBuilder;
            moveBaseExecute.In.BaseType = typeof(MockTask);
            moveBaseExecute.Execute();
        }

        static FieldBuilder CreateActionField(TypeBuilder typeBuilder)
        {
            var buildExecuteActionField = Task.New<CreateActionField>();
            buildExecuteActionField.In.TypeBuilder = typeBuilder;
            buildExecuteActionField.Execute();
            return buildExecuteActionField.Out.FieldBuilder;
        }
    }
}