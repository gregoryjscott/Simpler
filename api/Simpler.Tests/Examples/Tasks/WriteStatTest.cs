using Examples.Tasks;
using NUnit.Framework;
using Simpler;
using System;
using System.IO;

[TestFixture]
public class WriteStatTest
{
    [Test]
    public void output_includes_stat_attributes()
    {
        using (var stream = new MemoryStream())
        using (var reader = new StreamReader(stream))
        {
            var writeStat = Task.New<WriteStat>();
            writeStat.In.Stat = new Stat
            {
                Question = "The Question",
                Answer = "The Answer",
                Explanation = "The Reason"
            };
            writeStat.In.Stream = stream;
            writeStat.Execute();

            stream.Seek(0, SeekOrigin.Begin);
            var output = reader.ReadToEnd();

            Assert.That(output.Contains("The Question"));
            Assert.That((output.Contains("The Answer")));
            Assert.That((output.Contains("The Reason")));
        }
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
