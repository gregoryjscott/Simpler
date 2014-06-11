using System;
using System.Reflection.Emit;
using System.Reflection;

namespace Simpler
{
    public class CreateActionField : InOutTask<CreateActionField.Input, CreateActionField.Output>
    {
        public class Input
        {
            public TypeBuilder TypeBuilder { get; set; }
        }

        public class Output
        {
            public FieldBuilder FieldBuilder { get; set; }
        }

        public override void Execute()
        {
            Out.FieldBuilder = In.TypeBuilder.DefineField(
                "ExecuteAction",
                typeof(Action<Task>),
                FieldAttributes.Public
            );
        }
    }
}

