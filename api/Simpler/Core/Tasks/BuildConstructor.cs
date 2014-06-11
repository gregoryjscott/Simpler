using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Simpler
{
    public class BuildConstructor : InTask<BuildConstructor.Input>
    {
        public class Input
        {
            public TypeBuilder TypeBuilder { get; set; }
            public FieldInfo ExecuteOverrideField { get; set; }
        }

        public override void Execute()
        {
            var constructor = In.TypeBuilder.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                new[] { In.ExecuteOverrideField.FieldType }
            ).GetILGenerator();

            constructor.Emit(OpCodes.Ldarg_0);
            constructor.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));

            constructor.Emit(OpCodes.Ldarg_0);
            constructor.Emit(OpCodes.Ldarg_1);
            constructor.Emit(OpCodes.Stfld, In.ExecuteOverrideField);
            constructor.Emit(OpCodes.Ret);
        }
    }
}

