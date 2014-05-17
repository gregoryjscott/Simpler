using Examples.Tasks;
using NUnit.Framework;
using Simpler;
using System;
using System.IO;
using Examples.Models;

namespace Tests.Examples.Tasks
{
    [TestFixture]
    public class OutputStatTest
    {
        [Test]
        public void output_includes_stat_attributes()
        {
            var outputStat = Task.New<OutputStat>();
            outputStat.In.Stat = new Stat {
                Question = "The Question",
                Answer = "The Answer",
                Details = "The Details"
            };

            var output = ExecuteAndCaptureOutput(outputStat);

            Assert.That(output.Contains("The Question"));
            Assert.That((output.Contains("The Answer")));
            Assert.That((output.Contains("The Details")));
        }

        static string ExecuteAndCaptureOutput(Task task)
        {
            string output;

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            using (var reader = new StreamReader(stream))
            {
                var previousOut = Console.Out;
                Console.SetOut(writer);
                task.Execute();
                Console.SetOut(previousOut);

                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                output = reader.ReadToEnd();
            }

            return output;
        }
    }
}
