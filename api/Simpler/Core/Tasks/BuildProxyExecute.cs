using System.Reflection.Emit;
using System.Reflection;
using Simpler.Core.Tasks;
using System;

namespace Simpler
{
    public class BuildProxyExecute : InTask<BuildProxyExecute.Input>
    {
        public class Input
        {
            public TypeBuilder TypeBuilder { get; set; }
        }

        public override void Execute()
        {
            var execute = In.TypeBuilder.DefineMethod(
                "Execute",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual
            ).GetILGenerator();

            execute.Emit(OpCodes.Ldarg_0);
            execute.Emit(OpCodes.Dup);
            execute.Emit(OpCodes.Call, GetType().GetMethod("ProxyExecute"));
            execute.Emit(OpCodes.Ret);
        }

        public void ProxyExecute(Task task)
        {
            var executeTask = new ExecuteTask();
            executeTask.In.Task = task;
            executeTask.Execute();
        }
    }
}

