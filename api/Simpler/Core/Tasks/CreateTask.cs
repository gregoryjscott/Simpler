using System;
using System.Reflection.Emit;
using System.Reflection;
using System.Linq;

namespace Simpler.Core.Tasks
{
    public class CreateTask : InOutTask<CreateTask.Input, CreateTask.Output>
    {
        public class Input
        {
            public Type TaskType { get; set; }
        }

        public class Output
        {
            public object TaskInstance { get; set; }
        }

        public override void Execute()
        {
            var typeBuilder = CreateTypeBuilder(In.TaskType);

            BuildBaseExecuteMethod(typeBuilder, In.TaskType);
            BuildProxyExecuteMethod(typeBuilder);

            var proxyType = typeBuilder.CreateType();
            Out.TaskInstance = Activator.CreateInstance(proxyType);
        }

        #region Helpers

        static TypeBuilder CreateTypeBuilder(Type t)
        {
            var domain = AppDomain.CurrentDomain;
            var assemblyName = new AssemblyName("SimplerProxies");
            var assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, false);
            return moduleBuilder.DefineType(t.FullName + "Proxy", TypeAttributes.Public, t);
        }

        static void BuildBaseExecuteMethod(TypeBuilder typeBuilder, Type baseType)
        {
            var baseExecute = typeBuilder.DefineMethod(
                "BaseExecute",
                MethodAttributes.Public
            ).GetILGenerator();

            baseExecute.Emit(OpCodes.Ldarg_0);
            baseExecute.Emit(OpCodes.Call, baseType.GetMethod("Execute"));
            baseExecute.Emit(OpCodes.Ret);
        }

        static void BuildProxyExecuteMethod(TypeBuilder typeBuilder)
        {
            var execute = typeBuilder.DefineMethod(
                "Execute",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual
            ).GetILGenerator();

            execute.Emit(OpCodes.Ldarg_0);
            execute.Emit(OpCodes.Dup);
            execute.Emit(OpCodes.Call, typeof(CreateTask).GetMethod("ProxyExecute"));
            execute.Emit(OpCodes.Ret);
        }

        public void ProxyExecute(Task task)
        {
            var executeTask = new ExecuteTask();
            executeTask.In.Task = task;
            executeTask.Execute();
        }

        #endregion
    }
}
