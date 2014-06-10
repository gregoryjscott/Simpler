using System;
using System.Reflection.Emit;
using System.Reflection;

namespace Simpler
{
    public class MoveBaseExecute : InTask<MoveBaseExecute.Input>
    {
        public class Input
        {
            public TypeBuilder TypeBuilder { get; set; }
            public Type BaseType { get; set; }
        }

        public override void Execute()
        {
            var baseExecute = In.TypeBuilder.DefineMethod(
                "BaseExecute",
                MethodAttributes.Public
            ).GetILGenerator();

            baseExecute.Emit(OpCodes.Ldarg_0);
            baseExecute.Emit(OpCodes.Call, In.BaseType.GetMethod("Execute"));
            baseExecute.Emit(OpCodes.Ret);
        }
    }
}

