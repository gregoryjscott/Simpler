using System;
using System.Reflection;
using System.Reflection.Emit;
using NUnit.Framework;
using System.IO;

namespace Simpler.Tests.Core
{
    public class Normal : Task
    {
        public string Title { get { return "Normal"; } }
        public override void Execute()
        {
            Console.Write(0);
        }
    }

    public static class Base
    {
        public static object InvokeBase(this MethodInfo methodInfo, object targetObject)
        {
            var type = targetObject.GetType();
            var dynamicMethod = new DynamicMethod("OldExecute", null, new Type[] { type, typeof(Object) }, type);

            var iLGenerator = dynamicMethod.GetILGenerator();
            iLGenerator.Emit(OpCodes.Ldarg_0);
            iLGenerator.Emit(OpCodes.Call, methodInfo);
            iLGenerator.Emit(OpCodes.Ret);

            return dynamicMethod.Invoke(null, new object[] { targetObject, null });
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

        public void Whatever(Task task)
        {
            Console.Write(2);
            Type baseType = task.GetType().BaseType;
            baseType.GetMethod("Execute").InvokeBase(task);
            //((Normal)task).Execute();
            Console.Write(3);
        }

        static void OverrideExecute(TypeBuilder proxyTypeBuilder)
        {
            var executeBuilder = proxyTypeBuilder.DefineMethod("Execute", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual);
            var ilGenerator = executeBuilder.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Dup);
            ilGenerator.Emit(OpCodes.Call, typeof(CustomProxy).GetMethod("Whatever"));
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
            Assert.That(s.Title, Is.EqualTo("Normal"));
            Assert.That(output, Is.EqualTo("203"));
        }
    }

}