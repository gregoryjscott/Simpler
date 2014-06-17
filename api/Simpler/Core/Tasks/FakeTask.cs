﻿using System;
using System.Reflection.Emit;

namespace Simpler.Core.Tasks
{
    public class FakeTask : InOutTask<FakeTask.Input, FakeTask.Output>
    {
        public class Input
        {
            public Type TaskType { get; set; }
            public Action<Task> ExecuteOverride { get; set; }
        }

        public class Output
        {
            public object TaskInstance { get; set; }
        }

        public CreateProxyType CreateProxyType { get; set; }
        public BuildExecuteOverride BuildExecuteOverride { get; set; }

        public override void Execute()
        {
            if (CreateProxyType == null) CreateProxyType = new CreateProxyType();
            CreateProxyType.In.Type = In.TaskType;
            CreateProxyType.Execute();
            var typeBuilder = CreateProxyType.Out.TypeBuilder;

            if (BuildExecuteOverride == null) BuildExecuteOverride = new BuildExecuteOverride();
            BuildExecuteOverride.In.TypeBuilder = typeBuilder;
            BuildExecuteOverride.In.ExecuteOverride = In.ExecuteOverride;
            BuildExecuteOverride.Execute();

            var proxyType = typeBuilder.CreateType();
            Out.TaskInstance = Activator.CreateInstance(proxyType, In.ExecuteOverride);
        }
    }
}
