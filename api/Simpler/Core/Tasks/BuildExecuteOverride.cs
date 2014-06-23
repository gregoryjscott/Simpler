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
            public Action<Task> ExecuteOverride { get; set; }
        }

        public CreateActionField CreateActionField { get; set; }
        public BuildConstructor BuildConstructor { get; set; }

        public override void Execute()
        {
            if (CreateActionField == null) CreateActionField = new CreateActionField();
            CreateActionField.In.TypeBuilder = In.TypeBuilder;
            CreateActionField.Execute();
            var actionField = CreateActionField.Out.FieldBuilder;

            if (BuildConstructor == null) BuildConstructor = new BuildConstructor();
            BuildConstructor.In.TypeBuilder = In.TypeBuilder;
            BuildConstructor.In.ActionFieldInfo = actionField;
            BuildConstructor.Execute();

            // Override the existing Execute method.
            var execute = In.TypeBuilder.DefineMethod(
                "Execute",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual
            ).GetILGenerator();

            // Make the override Execute call ExecuteOverride. 
            execute.Emit(OpCodes.Ldarg_0);
            execute.Emit(OpCodes.Dup);
            execute.Emit(OpCodes.Call, GetType().GetMethod("ExecuteOverride"));
            execute.Emit(OpCodes.Ret);
        }

        public void ExecuteOverride(Task task)
        {
            var action = GetType().GetField("ExecuteAction").GetValue(task);
            ((Action<Task>)action)(task);
        }
    }
}
