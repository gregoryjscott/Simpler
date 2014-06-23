using NUnit.Framework;
using Simpler;
using Simpler.Mocks;
using System.Reflection.Emit;
using System;

namespace Simpler.Core.Tasks
{
    [TestFixture]
    public class SaveBaseExecuteTest
    {
        [Test]
        public void should_move_base_execute()
        {
            var proxyTypeBuilder = CreateProxyTypeTest.CreateProxyTypeBuilder();

            SaveBaseExecute(proxyTypeBuilder);

            var proxyType = proxyTypeBuilder.CreateType();
            var proxy = (MockTask)Activator.CreateInstance(proxyType);

            proxyType.GetMethod("BaseExecute").Invoke(proxy, null);
            Assert.That(proxy.WasExecuted, Is.True);
        }

        public static void SaveBaseExecute(TypeBuilder typeBuilder)
        {
            var moveBaseExecute = Task.New<SaveBaseExecute>();
            moveBaseExecute.In.TypeBuilder = typeBuilder;
            moveBaseExecute.In.BaseType = typeof(MockTask);
            moveBaseExecute.Execute();
        }
    }
}
