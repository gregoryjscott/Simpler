using System;
using System.Reflection;
using System.Reflection.Emit;
using NUnit.Framework;
using System.IO;

namespace Simpler.Tests.Core
{
    public class CustomProxy
    {
        public static T New<T>()
        {
            var typeBuilder = CreateTypeBuilder<T>();
            AddBaseExecute(typeBuilder, typeof(T));
            AddProxyExecute(typeBuilder, typeof(T));
            var proxyType = typeBuilder.CreateType();
            return (T)Activator.CreateInstance(proxyType);
        }

        static TypeBuilder CreateTypeBuilder<T>()
        {
            var domain = AppDomain.CurrentDomain;
            var assemblyName = new AssemblyName("SimplerProxies");
            var assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, false);
            return moduleBuilder.DefineType(typeof(T).FullName + "Proxy", TypeAttributes.Public, typeof(T));
        }

        static void AddBaseExecute(TypeBuilder typeBuilder, Type t)
        {
            var baseExecute = typeBuilder.DefineMethod(
                "BaseExecute", 
                MethodAttributes.Public
            ).GetILGenerator();

            baseExecute.Emit(OpCodes.Ldarg_0);
            baseExecute.Emit(OpCodes.Call, t.GetMethod("Execute"));
            baseExecute.Emit(OpCodes.Ret);
        }

        static void AddProxyExecute(TypeBuilder typeBuilder, Type t)
        {
            var proxyExecute = typeBuilder.DefineMethod(
                "Execute", 
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual
            ).GetILGenerator();

            proxyExecute.Emit(OpCodes.Ldarg_0);
            proxyExecute.Emit(OpCodes.Dup);
            proxyExecute.Emit(OpCodes.Call, typeof(CustomProxy).GetMethod("ProxyExecute"));
            proxyExecute.Emit(OpCodes.Ret);
        }

        public void ProxyExecute(Task task)
        {
            Console.Write(1);
            task.GetType().GetMethod("BaseExecute").Invoke(task, null);
            Console.Write(3);
        }
    }

    public class SayHello : InOutTask<SayHello.Input, SayHello.Output>
    {
        public class Input
        {
            public string Name { get; set; }
        }

        public class Output
        {
            public string Response { get; set; }
        }

        public override void Execute()
        {
            Console.Write(2);
            Out.Response = String.Format("Hello {0}.", In.Name);
        }
    }

    [TestFixture]
    public class CustomProxyTest
    {
        [Test]
        public void creates_instance_of_given_type()
        {
            var proxy = CustomProxy.New<SayHello>();
            Assert.That(proxy, Is.InstanceOf<SayHello>());
        }

        [Test]
        public void creates_class_with_same_name_plus_Proxy()
        {
            var proxy = CustomProxy.New<SayHello>();
            Assert.That(proxy.GetType().FullName, Is.EqualTo("Simpler.Tests.Core.SayHelloProxy"));
        }

        [Test]
        public void overrides_normal_execute_with_different_execute()
        {
            var sayHello = CustomProxy.New<SayHello>();
            sayHello.In.Name = "Greg";

            string output;
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                sayHello.Execute();
                output = sw.ToString();
            }
            Assert.That(output, Is.EqualTo("123"));
            Assert.That(sayHello.Out.Response, Is.EqualTo("Hello Greg."));
        }
    }
}