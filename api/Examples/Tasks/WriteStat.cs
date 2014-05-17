using System.IO;
using Simpler;
using System;
using System.Text;

namespace Examples.Tasks
{
    public class WriteStat: InTask<WriteStat.Input>
    {
        public class Input
        {
            public Stat Stat { get; set; }
            public Stream Stream { get; set; }
        }

        public override void Execute()
        {
            var builder = new StringBuilder();
            builder.Append(String.Format("Question: {0}", In.Stat.Question));
            builder.Append(Environment.NewLine);
            builder.Append(String.Format("Answer: {0}", In.Stat.Answer));
            builder.Append(Environment.NewLine);
            builder.Append(String.Format("Reason: {0}", In.Stat.Explanation));

            using (var writer = new StreamWriter(In.Stream))
            {
                writer.WriteLine(builder);

            }
        }
    }
}
