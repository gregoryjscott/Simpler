using System;
using System.Data;
using Simpler.Mocks;

namespace Simpler.Data.Jobs
{
    public class ExecuteAction : InJob<ExecuteAction.Input>
    {
        public override void Specs()
        {
            It<ExecuteAction>.Should(
                "create a command and pass it to the given action",
                it =>
                {
                    var actionWasPassedACommand = false;
                    it.In.Connection = new MockConnection();
                    it.In.Sql = "select ...";
                    it.In.Values = new { Something = "nothing" };
                    it.In.Action = command => { if (command != null) actionWasPassedACommand = true; };
                    it.BuildParameters = Fake.Job<BuildParameters>();

                    it.Run();

                    Check.That(actionWasPassedACommand, "The given action should have been passed a command object.");
                });

            It<ExecuteAction>.Should(
                "build parameters using given values",
                it =>
                {
                    var buildParametersCalled = false;
                    it.In.Connection = new MockConnection();
                    it.In.Sql = "select ...";
                    it.In.Values = new { Something = "nothing" };
                    it.In.Action = command => { };
                    it.BuildParameters = Fake.Job<BuildParameters>(job => buildParametersCalled = true);

                    it.Run();

                    Check.That(buildParametersCalled, "Expected parameters to be built using given values.");
                });
        }

        public class Input
        {
            public IDbConnection Connection { get; set; }
            public string Sql { get; set; }
            public object Values { get; set; }
            public Action<IDbCommand> Action { get; set; }
        }

        public BuildParameters BuildParameters { get; set; }

        public override void Run()
        {
            Check.That(!String.IsNullOrEmpty(In.Sql), "Sql property must be set.");

            using (var command = In.Connection.CreateCommand())
            {
                if (In.Connection.State != ConnectionState.Open)
                {
                    In.Connection.Open();
                }

                command.Connection = In.Connection;
                command.CommandText = In.Sql;

                if (In.Values != null)
                {
                    BuildParameters.Command = command;
                    BuildParameters.Values = In.Values;
                    BuildParameters.Run();
                }

                In.Action(command);
            }
        }
    }
}
