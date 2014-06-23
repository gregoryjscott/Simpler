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
            public FieldInfo ActionFieldInfo { get; set; }
        }

        public override void Execute()
        {
            var constructor = In.TypeBuilder.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                new[] { typeof(Action<Task>) }
            ).GetILGenerator();

            // Call the base constructor.
            constructor.Emit(OpCodes.Ldarg_0);
            constructor.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));

            // Store the constructor parameter in a field.
            constructor.Emit(OpCodes.Ldarg_0);
            constructor.Emit(OpCodes.Ldarg_1);
            constructor.Emit(OpCodes.Stfld, In.ActionFieldInfo);
            constructor.Emit(OpCodes.Ret);
        }
    }
}
