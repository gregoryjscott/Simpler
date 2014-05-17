using Simpler;
using System;
using System.Text;
using Baseball.Models;

namespace Baseball.Tasks
{
    public class OutputStat: InTask<OutputStat.Input>
    {
        public class Input
        {
            public Stat Stat { get; set; }
        }

        public override void Execute()
        {
            var builder = new StringBuilder();
            builder.Append(String.Format("Question: {0}", In.Stat.Question));
            builder.Append(Environment.NewLine);
            builder.Append(String.Format("  Answer: {0}", In.Stat.Answer));
            builder.Append(Environment.NewLine);
            builder.Append(String.Format("  Reason: {0}", In.Stat.Details));
            Console.WriteLine(builder);
        }
    }
}
