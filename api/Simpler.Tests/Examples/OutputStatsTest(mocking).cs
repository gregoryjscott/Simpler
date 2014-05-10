using NUnit.Framework;
using Simpler;
using System;
using System.IO;

[TestFixture]
public class OutputStatsTest
{
    [Test]
    public void outputs_team_count()
    {
        var stub = CreateOutputStatsStub();
        var output = ExecuteAndCaptureOutput(stub);
        Assert.That(output.Contains(" 2 teams"));
    }

    [Test]
    public void outputs_Cardinals_manager()
    {
        var stub = CreateOutputStatsStub();
        var output = ExecuteAndCaptureOutput(stub);
        Assert.That(output.Contains("Mike Matheny"));
    }

    [Test]
    public void returns_0_if_all_goes_well()
    {
        var stub = CreateOutputStatsStub();
        ExecuteAndCaptureOutput(stub);
        Assert.That(stub.Out.ReturnValue, Is.EqualTo(0));
    }

    [Test]
    public void returns_50_if_tables_check_fails()
    {
        var stub = CreateOutputStatsStub();
        stub.CheckTables = Fake.Task<CheckTables>(ct => {throw new Exception();});
        ExecuteAndCaptureOutput(stub);
        Assert.That(stub.Out.ReturnValue, Is.EqualTo(50));
    }

    static OutputStats CreateOutputStatsStub()
    {
        var outputStats = Task.New<OutputStats>();
        outputStats.CheckTables = Fake.Task<CheckTables>();
        outputStats.FetchTeams = Fake.Task<FetchTeams>(
            ft => ft.Out.Teams = new[] {
                new Team { Name = "Cardinals" },
                new Team { Name = "Cubs" }
            });
        outputStats.FetchManager = Fake.Task<FetchManager>(
            fm => fm.Out.Manager = new Manager { Name = "Mike Matheny" });
        return outputStats;
    }

    static string ExecuteAndCaptureOutput(Task task)
    {
        string output;
        var previousOut = Console.Out;

        using (var stream = new MemoryStream())
        using (var writer = new StreamWriter(stream))
        using (var reader = new StreamReader(stream))
        {
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
