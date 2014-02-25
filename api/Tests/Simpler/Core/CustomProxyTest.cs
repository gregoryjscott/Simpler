using System;
using System.Reflection;
using System.Reflection.Emit;
using NUnit.Framework;

namespace Simpler.Tests.Core
{
    public class Normal
    {
        public virtual void Execute()
        {
            Console.WriteLine("Normal.Execute()");
        }
    }

    public class Different
    {
        public static void Thing()
        {
            Console.WriteLine("Different.Thing()");
        }
    }

    public class CustomProxy
    {
        public static T New<T>()
        {
            var domain = AppDomain.CurrentDomain;
            var assemblyName = new AssemblyName("SimplerProxies");
            var assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, false);

            var typeBuilder = moduleBuilder.DefineType(typeof(Normal).FullName + "Proxy", TypeAttributes.Public, typeof(T));

            var executeBuilder = typeBuilder.DefineMethod("Execute", MethodAttributes.Public);
            var executeILGenerator = executeBuilder.GetILGenerator();
            executeILGenerator.Emit(OpCodes.Call, typeof(Different).GetMethod("Thing"));

            var proxy = typeBuilder.CreateType();
            return (T)Activator.CreateInstance(proxy);
        }
    }

    [TestFixture]
    public class CustomProxyTest
    {
        [Test]
        public void creates_instance_of_given_type()
        {
            var proxy = CustomProxy.New<Normal>();
            Assert.That(proxy, Is.InstanceOf<Normal>());
        }

        [Test]
        public void creates_class_with_same_name_plus_Proxy()
        {
            var proxy = CustomProxy.New<Normal>();
            Assert.That(proxy.GetType().FullName, Is.EqualTo("Simpler.Tests.Core.NormalProxy"));
        }

        [Test]
        public void play_with_overriding_execute()
        {
            // This writes "Different.Thing()" as expected.
            dynamic d = CustomProxy.New<Normal>();
            d.Execute();

            // This writes "Normal.Execute()". Why?
            Normal s = CustomProxy.New<Normal>();
            s.Execute();
        }
    }
}