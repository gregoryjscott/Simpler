using System;
using System.Reflection.Emit;
using System.Reflection;

namespace Simpler
{
    public class SaveBaseExecute : InTask<SaveBaseExecute.Input>
    {
        public class Input
        {
            public TypeBuilder TypeBuilder { get; set; }
            public Type BaseType { get; set; }
        }

        public override void Execute()
        {
            // Create new method called BaseExecute.
            var baseExecute = In.TypeBuilder.DefineMethod(
                "BaseExecute",
                MethodAttributes.Public
            ).GetILGenerator();

            // Make it call the base version of Execute.
            baseExecute.Emit(OpCodes.Ldarg_0);
            baseExecute.Emit(OpCodes.Call, In.BaseType.GetMethod("Execute"));
            baseExecute.Emit(OpCodes.Ret);
        }
    }
}
