using NUnit.Framework;

namespace Simpler
{
    /// <summary>
    /// The foundation of Simpler.
   /// </summary>
    public abstract class Task
    {
        /// <summary>
        /// Container for the data the task needs to execute.
        /// </summary>
        /// <example>
        /// task.Inputs.Question = "Is this cool?";
        /// </example>
        public dynamic Inputs { get; set; }

        /// <summary>
        /// Container for the data the task produces.
        /// </summary>
        /// <example>
        /// var answer = task.Outputs.Answer;
        /// </example>
        public dynamic Outputs { get; set; }

        /// <summary>
        /// Code that uses the Inputs to produce the Outputs.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Shorthand method for setting the inputs, executing, and receiving the outputs in one line of code.
        /// </summary>
        /// <example>
        /// var answer = task.Execute(new { Question = "Is this cool?" }).Outputs.Answer;
        /// </example>
        /// <param name="inputs">Used to set Inputs prior to execution.</param>
        /// <returns>The Outputs</returns>
        public dynamic Execute(dynamic inputs)
        {
            Inputs = inputs;
            Execute();
            return Outputs;
        }

        /// <summary>
        /// Should define and add tests for this task.  The tests should verify the task handles Inputs correctly, and produces Outputs correctly,
        /// for all business cases.
        /// 
        /// This method is virtual instead of abstract, therefore it is optional, but there's no excuse to not use it.
        /// </summary>
        public virtual void Tests() { }

        // Example declaration.
        class AskTheNewSyntaxAQuestion : Task
        {
            public override void Execute()
            {
                if (Inputs.Question == "Is this cool?")
                {
                    Outputs = new
                    {
                        Answer = "Yes."
                    };
                }
                else
                {
                    Outputs = new
                    {
                        Answer = "I dont know."
                    };
                }
            }

            public override void Tests()
            {
                Test.Add(new Test
                {
                    Expectation = "should answer 'Yes.' to 'Is this cool?'",

                    Setup = (task) =>
                    {
                        task.Inputs.Question = "Is this cool?";
                    },

                    Verify = (task) => Assert.That(task.Outputs.Answer).IsEqualTo("Yes.")
                });

                Test.Add(new Test
                {
                    Expectation = "should answer 'I dont know.' to 'Will it work?'",

                    Setup = (task) =>
                    {
                        task.Inputs.Question = "Will it work?";
                    },

                    Verify = (task) => Assert.That(task.Outputs.Answer).IsEqualTo("I dont know.")
                });
            }
        }
    }
}