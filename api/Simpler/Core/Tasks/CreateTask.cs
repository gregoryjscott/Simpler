using System;
using System.Reflection.Emit;

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
        public BuildProxyExecute BuildProxyExecute { get; set; }

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

            if (BuildProxyExecute == null) BuildProxyExecute = new BuildProxyExecute();
            BuildProxyExecute.In.TypeBuilder = typeBuilder;
            BuildProxyExecute.Execute();

            var proxyType = typeBuilder.CreateType();
            Out.TaskInstance = Activator.CreateInstance(proxyType);
        }
    }
}
