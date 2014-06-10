using System;
using System.Reflection.Emit;
using System.Reflection;

namespace Simpler.Core.Tasks
{
    public class FakeTask : InOutTask<FakeTask.Input, FakeTask.Output>
    {
        public class Input
        {
            public Type TaskType { get; set; }
            public Action<Task> ExecuteOverride { get; set; }
        }

        public class Output
        {
            public object TaskInstance { get; set; }
        }

        public CreateProxyType CreateProxyType { get; set; }

        public override void Execute()
        {
            if (CreateProxyType == null) CreateProxyType = new CreateProxyType();
            CreateProxyType.In.Type = In.TaskType;
            CreateProxyType.Execute();
            var typeBuilder = CreateProxyType.Out.TypeBuilder;

            var actionField = BuildExecuteOverrideActionField(typeBuilder);
            BuildConstructor(typeBuilder, actionField);
            BuildExecuteOverrideMethod(typeBuilder, actionField);

            var proxyType = typeBuilder.CreateType();
            Out.TaskInstance = Activator.CreateInstance(proxyType, In.ExecuteOverride);
        }

        #region Helpers

        static FieldBuilder BuildExecuteOverrideActionField(TypeBuilder typeBuilder)
        {
            return typeBuilder.DefineField("ExecuteAction", typeof(Action<Task>), FieldAttributes.Public);
        }

        static void BuildConstructor(TypeBuilder typeBuilder, FieldInfo executeOverrideField)
        {
            var constructor = typeBuilder.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                new[] { executeOverrideField.FieldType }
            ).GetILGenerator();

            constructor.Emit(OpCodes.Ldarg_0);
            constructor.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));

            constructor.Emit(OpCodes.Ldarg_1);
            constructor.Emit(OpCodes.Stfld, executeOverrideField);
            constructor.Emit(OpCodes.Ret);
        }

        static void BuildExecuteOverrideMethod(TypeBuilder typeBuilder, FieldInfo fakeExecuteField)
        {
            var execute = typeBuilder.DefineMethod(
                "Execute",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual
            ).GetILGenerator();

            execute.Emit(OpCodes.Ldarg_0);
            execute.Emit(OpCodes.Dup);
            execute.Emit(OpCodes.Ldfld, fakeExecuteField);
            execute.Emit(OpCodes.Call, typeof(CreateTask).GetMethod("ExecuteOverride"));
            execute.Emit(OpCodes.Ret);
        }

        public void ExecuteOverride(Task task, Action<Task> executeAction)
        {
            executeAction(task);
        }

        #endregion
    }
}
