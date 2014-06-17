using System;
using System.Reflection.Emit;
using System.Reflection;

namespace Simpler
{
    public class BuildExecuteOverride : InTask<BuildExecuteOverride.Input>
    {
        public class Input
        {
            public TypeBuilder TypeBuilder { get; set; }
            //public FieldInfo ActionField { get; set; }
            //public MethodInfo MethodInfo { get; set; }
            public Action<Task> ExecuteOverride { get; set; }
        }

        public override void Execute()
        {
            var execute = In.TypeBuilder.DefineMethod(
                "Execute",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual
            ).GetILGenerator();

            execute.Emit(OpCodes.Ldarg_0);
            execute.Emit(OpCodes.Dup);
            //execute.Emit(OpCodes.Ldobj, In.ActionField);
            //execute.Emit(OpCodes.Unbox_Any, typeof(int));
            execute.Emit(OpCodes.Call, GetType().GetMethod("ExecuteOverride"));
            execute.Emit(OpCodes.Ret);
        }

        public void ExecuteOverride(Task task)
        {
            var action = GetType().GetField("ExecuteAction").GetValue(task);
            ((Action<Task>)action)(task);
            //task.ExecuteAction(task);
        }
    }
}

