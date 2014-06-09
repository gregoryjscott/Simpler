using System;
using System.Reflection;
using System.Reflection.Emit;
using NUnit.Framework;
using System.IO;

namespace Simpler.Tests.Core
{
    public class Normal
    {
        public virtual void Execute()
        {
            Console.Write("Normal.Execute()");
        }
    }

    public class Different
    {
        public virtual void Thing()
        {
            Console.Write("Different.Thing()");
        }
    }

    public class CustomProxy
    {
        public static T New<T>()
        {
            var proxyTypeBuilder = CreateTypeBuilderFromT<T>();

            OverrideExecute(proxyTypeBuilder);

            var proxy = proxyTypeBuilder.CreateType();
            return (T)Activator.CreateInstance(proxy);
        }

        static void OverrideExecute(TypeBuilder proxyTypeBuilder)
        {
            var executeBuilder = proxyTypeBuilder.DefineMethod("Execute", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual);
            var ilGenerator = executeBuilder.GetILGenerator();

            // This isn't the desired behavior - just experimenting. This is trying to override 
            // Normal.Execute() with a Execute() method that calls Different.Thing().
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Call, typeof (Different).GetMethod("Thing"));
            ilGenerator.Emit(OpCodes.Ret);
        }

        static TypeBuilder CreateTypeBuilderFromT<T>()
        {
            var domain = AppDomain.CurrentDomain;
            var assemblyName = new AssemblyName("SimplerProxies");
            var assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, false);
            return moduleBuilder.DefineType(typeof(Normal).FullName + "Proxy", TypeAttributes.Public, typeof(T));
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
        public void overrides_normal_execute_with_different_execute()
        {
            Normal s = CustomProxy.New<Normal>();

            string output;
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                s.Execute();
                output = sw.ToString();
            }
            Assert.That(output, Is.EqualTo("Different.Thing()"));
        }
    }
}