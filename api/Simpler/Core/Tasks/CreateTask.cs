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

        public CreateProxyType CreateProxyType { get; set; }
        public MoveBaseExecute MoveBaseExecute { get; set; }

        public override void Execute()
        {
            if (CreateProxyType == null) CreateProxyType = new CreateProxyType();
            CreateProxyType.In.Type = In.TaskType;
            CreateProxyType.Execute();
            var typeBuilder = CreateProxyType.Out.TypeBuilder;

            if (MoveBaseExecute == null) MoveBaseExecute = new MoveBaseExecute();
            MoveBaseExecute.In.TypeBuilder = typeBuilder;
            MoveBaseExecute.In.BaseType = In.TaskType;
            MoveBaseExecute.Execute();

            BuildProxyExecuteMethod(typeBuilder);

            var proxyType = typeBuilder.CreateType();
            Out.TaskInstance = Activator.CreateInstance(proxyType);
        }

        #region Helpers

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
