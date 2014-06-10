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
            public Action<Task> FakeExecute { get; set; }
        }

        public class Output
        {
            public object TaskInstance { get; set; }
        }

        public ExecuteTask ExecuteTask { get; set; }

        public override void Execute()
        {
            var typeBuilder = CreateTypeBuilder(In.TaskType);
//            var baseExecute = (In.FakeExecute == null)
//                ? In.TaskType.GetMethod("Execute")
//                : In.FakeExecute.Method;
            BuildBaseExecute(typeBuilder, In.TaskType);
            BuildProxyExecute(typeBuilder);

            if (In.FakeExecute == null)
            {
                var proxyType = typeBuilder.CreateType();
                Out.TaskInstance = Activator.CreateInstance(proxyType);
            }
            else
            {
                var fakeExecuteField = BuildFakeExecuteField(typeBuilder);
                BuildConstructor(typeBuilder, fakeExecuteField);
                var proxyType = typeBuilder.CreateType();
                Out.TaskInstance = Activator.CreateInstance(proxyType, In.FakeExecute);
            }
        }

        #region Helpers

//        static MethodInfo CreateFakeExecute(Action<Task> fakeAction)
//        {
//            var fakeExecuteMethod = new DynamicMethod("FakeExecute", null, null);
//            var fakeExecute = fakeExecuteMethod.GetILGenerator();
//
//            fakeExecute.Emit(OpCodes.Ldarg_0);
//            fakeExecute.Emit(OpCodes.Call, fakeAction.Method);
//            fakeExecute.Emit(OpCodes.Ret);
//            return fakeExecuteMethod;
//        }

        static TypeBuilder CreateTypeBuilder(Type t)
        {
            var domain = AppDomain.CurrentDomain;
            var assemblyName = new AssemblyName("SimplerProxies");
            var assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, false);
            return moduleBuilder.DefineType(t.FullName + "Proxy", TypeAttributes.Public, t);
        }

        static void BuildBaseExecute(TypeBuilder typeBuilder, Type baseType)
        {
            var baseExecute = typeBuilder.DefineMethod(
                "BaseExecute",
                MethodAttributes.Public
            ).GetILGenerator();

            baseExecute.Emit(OpCodes.Ldarg_0);
            baseExecute.Emit(OpCodes.Call, baseType.GetMethod("Execute"));
            baseExecute.Emit(OpCodes.Ret);
        }

        static void BuildProxyExecute(TypeBuilder typeBuilder)
        {
            var proxyExecute = typeBuilder.DefineMethod(
                "Execute",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual
            ).GetILGenerator();

            proxyExecute.Emit(OpCodes.Ldarg_0);
            proxyExecute.Emit(OpCodes.Dup);
            proxyExecute.Emit(OpCodes.Call, typeof(CreateTask).GetMethod("ProxyExecute"));
            proxyExecute.Emit(OpCodes.Ret);
        }

        public void ProxyExecute(Task task)
        {
            var executeTask = new ExecuteTask();
            executeTask.In.Task = task;
            executeTask.Execute();
        }

        static FieldBuilder BuildFakeExecuteField(TypeBuilder typeBuilder)
        {
            return typeBuilder.DefineField("FakeExecute", typeof(Action<Task>), FieldAttributes.Public);
        }

        static void BuildConstructor(TypeBuilder typeBuilder, params FieldBuilder[] parameters)
        {
            ConstructorBuilder ctor = typeBuilder.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                parameters.Select(f => f.FieldType).ToArray()
            );

            // Emit constructor
            ILGenerator g = ctor.GetILGenerator();

            // Load "this" pointer and call base constructor
            g.Emit(OpCodes.Ldarg_0);
            g.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));

            // Store parameters in private fields
            for (int i = 0; i < parameters.Length; i++)
            {
                // Load "this" pointer and parameter and store paramater in private field
                g.Emit(OpCodes.Ldarg_0);
                g.Emit(OpCodes.Ldarg, i + 1);
                g.Emit(OpCodes.Stfld, parameters[i]);
            }

            // Return
            g.Emit(OpCodes.Ret);
        }

        static void BuildFakeExecute(TypeBuilder typeBuilder, FieldInfo fakeExecuteField)
        {
            var fakeExecute = typeBuilder.DefineMethod(
                "Execute",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual
            ).GetILGenerator();

            fakeExecute.Emit(OpCodes.Ldarg_0);
            fakeExecute.Emit(OpCodes.Ldfld, fakeExecuteField);
            fakeExecute.Emit(OpCodes.Call, typeof(CreateTask).GetMethod("FakeExecute"));
            fakeExecute.Emit(OpCodes.Ret);
        }

        public void FakeExecute(Task task, Action<Task> executeAction)
        {
            executeAction(task);
        }

        #endregion
    }
}
