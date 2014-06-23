using NUnit.Framework;
using Simpler;
using System.Reflection.Emit;
using System;
using System.Reflection;

namespace Simpler.Core.Tasks
{
    [TestFixture]
    public class CreateActionFieldTest
    {
        [Test]
        public void should_create_action_field()
        {
            var proxyTypeBuilder = CreateProxyTypeTest.CreateProxyTypeBuilder();
            CreateActionField(proxyTypeBuilder);

            var proxyType = proxyTypeBuilder.CreateType();

            var actionField = proxyType.GetField("ExecuteAction");
            Assert.That(
                actionField.FieldType.FullName,
                Contains.Substring("Action`1").And.ContainsSubstring("Simpler.Task")
            );
        }

        public static FieldBuilder CreateActionField(TypeBuilder typeBuilder)
        {
            var buildExecuteActionField = Task.New<CreateActionField>();
            buildExecuteActionField.In.TypeBuilder = typeBuilder;
            buildExecuteActionField.Execute();
            return buildExecuteActionField.Out.FieldBuilder;
        }
    }
}