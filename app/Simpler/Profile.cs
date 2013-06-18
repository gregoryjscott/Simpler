using System;
using System.Collections.Generic;
using System.Linq;
using Simpler.Core.Interfaces;

namespace Simpler
{
    public class Profile
    {
        static void ProcessResults(ITask task, Dictionary<string, double> results)
        {
            var average = results.Average(tr => tr.Value);
            Console.WriteLine(
                Environment.NewLine +
                "{0}: The average duration was {1} milliseconds and the 10 worst are listed below.",
                task.Name,
                average);

            var count = 0;
            var worst = results.OrderByDescending(tr => tr.Value).Take(10);
            foreach (var result in worst)
            {
                count++;
                Console.WriteLine(Environment.NewLine + "    {0}. {1}: {2} milliseconds",
                                  count,
                                  result.Key,
                                  result.Value);
            }
        }

        public static void Task<TTask, TIn>(TTask task, TIn[] inputs) where TTask : IInTask<TIn>
        {
            var dict = new Dictionary<string, double>();

            foreach (var input in inputs)
            {
                for (var i = 0; i < 10; i++)
                {
                    task.In = input;
                    task.Execute();
                }

                var average = task.Stats.ExecuteDurations
                    .OrderBy(e => e.Duration().TotalMilliseconds)
                    .Skip(1)
                    .Take(8)
                    .Average(e => e.Duration().TotalMilliseconds);

                dict.Add(task.ToString(), average);

                task.Stats.ExecuteDurations.Clear();
            }

            ProcessResults(task, dict);
        }
    }
}
