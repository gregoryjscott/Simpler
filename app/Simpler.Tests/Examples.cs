using System;
using System.Data.SqlClient;
using NUnit.Framework;
using Simpler;
using Simpler.Data;
using Simpler.Data.Tasks;

public class WriteName : Task
{
    public override void Execute()
    {
        Console.WriteLine("{0} here.", Name);
    }
}

public class WriteNameProgram
{
    WriteNameProgram()
    {
        var writeName = Task.New<WriteName>();
        writeName.Execute(); // outputs "WriteName here."
        Console.WriteLine(writeName.Stats.ExecuteCount); // outputs "1"
    }
}

public class TellSecret : InTask<TellSecret.Input>
{
    public class Input
    {
        public string Secret { get; set; }
    }

    public override void Execute()
    {
        // Do something with secret.
    }
}

public class ReceiveAdvice : OutTask<ReceiveAdvice.Output>
{
    public class Output
    {
        public string Advice { get; set; }
    }

    public override void Execute()
    {
        // Find some good advice.
    }
}

public class AskDumbQuestion : InOutTask<AskDumbQuestion.Input, AskDumbQuestion.Output>
{
    public class Input
    {
        public string Question { get; set; }
    }

    public class Output
    {
        public string Answer { get; set; }
    }

    public override void Execute()
    {
        Out.Answer =
            In.Question == "Is this cool?"
                ? "Definitely."
                : "Get a life.";
    }
}

public class BeAnnoying : InTask<BeAnnoying.Input>
{
    public class Input
    {
        public int AnnoyanceLevel { get; set; }
    }

    // Sub-task
    public AskDumbQuestion AskDumbQuestion { get; set; }

    public override void Execute()
    {
        // Notice that AskDumbQuestion was automatically instantiated.
        AskDumbQuestion.In.Question = "Is this cool?";

        for (var i = 0; i < In.AnnoyanceLevel; i++)
        {
            AskDumbQuestion.Execute();
        }
    }
}

public class CheckSnowReport : InOutTask<CheckSnowReport.Input, CheckSnowReport.Output>
{
    public class Input
    {
        public DateTime DateTime { get; set; }
    }

    public class Output
    {
        public int InchesOfSnow { get; set; }
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}

[TestFixture]
public class CheckSnowReportTest
{
    [Test]
    public void should_find_snow_Xmas_1982()
    {
        // Arrange
        var checkSnowReport = Task.New<CheckSnowReport>();
        checkSnowReport.In.DateTime = new DateTime(1982, 12, 25);

        // Act
        checkSnowReport.Execute();

        // Assert
        Assert.That(checkSnowReport.Out.InchesOfSnow, Is.GreaterThan(0));
    }
}

public class GoSkiing : OutTask<GoSkiing.Output>
{
    public class Output
    {
        public bool Yes { get; set; }
    }

    public CheckSnowReport CheckSnowReport { get; set; }

    public override void Execute()
    {
        CheckSnowReport.Execute();
        Out.Yes = CheckSnowReport.Out.InchesOfSnow >= 6;
    }
}

[TestFixture]
public class GoSkiingTest
{
    [Test]
    public void should_go_skiing_on_powder_days()
    {
        // Arrange
        var goSkiing = Task.New<GoSkiing>();
        goSkiing.CheckSnowReport = Fake.Task<CheckSnowReport>(csr => csr.Out.InchesOfSnow = 6);

        // Act
        goSkiing.Execute();

        // Assert
        Assert.That(goSkiing.Out.Yes, Is.True);
    }
}

public class FlightInfo
{
    public string Location { get; set; }
    public string Destination { get; set; }
    public DateTime Depart { get; set; }
    public DateTime Arrival { get; set; }
}

public class CheckUnited : InOutTask<CheckUnited.Input, CheckUnited.Output>
{
    public class Input
    {
        public FlightInfo FlightInfo { get; set; }
    }
    public class Output
    {
        public decimal Price { get; set; }
        public string Url { get; set; }
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}
public class CheckDelta : CheckUnited { }

namespace Simpler
{
    public static class Parallel
    {
        public static void Execute(params Task[] tasks)
        {
            System.Threading.Tasks.Parallel.ForEach(tasks, task => task.Execute());
        }
    }
}

public class FindFlight : InOutTask<FindFlight.Input, FindFlight.Output>
{
    public class Input
    {
        public FlightInfo FlightInfo { get; set; }
    }

    public class Output
    {
        public string Url { get; set; }
    }

    public CheckUnited CheckUnited { get; set; }
    public CheckDelta CheckDelta { get; set; }

    public override void Execute()
    {
        CheckUnited.In.FlightInfo = In.FlightInfo;
        CheckDelta.In.FlightInfo = In.FlightInfo;

        Parallel.Execute(CheckUnited, CheckDelta);

        Out.Url = CheckUnited.Out.Price < CheckDelta.Out.Price 
            ? CheckUnited.Out.Url 
            : CheckDelta.Out.Url;
    }
}

public class Stuff
{
    public string Name { get; set; }
}

public class FetchCertainStuff : InOutTask<FetchCertainStuff.Input, FetchCertainStuff.Output>
{
    public class Input
    {
        public string SomeCriteria { get; set; }
    }

    public class Output
    {
        public Stuff[] Stuff { get; set; }
    }

    public override void Execute()
    {
        using(var connection = Db.Connect("MyConnectionString"))
        {
            const string sql =
                @"
                select 
                    AColumn as Name
                from 
                    ABunchOfJoinedTables
                where 
                    SomeColumn = @SomeCriteria
                    and
                    AnotherColumn = @SomeOtherCriteria
                ";

            var values = new {In.SomeCriteria, SomeOtherCriteria = "other criteria"};

            Out.Stuff = Db.GetMany<Stuff>(connection, sql, values);
        }
    }
}

public class LogAttribute : EventsAttribute
{
    public override void BeforeExecute(Task task)
    {
        Console.WriteLine("{0} started.", task.Name);
    }

    public override void AfterExecute(Task task)
    {
        Console.WriteLine("{0} finished.", task.Name);
    }

    public override void OnError(Task task, Exception exception)
    {
        Console.WriteLine("{0} bombed.", task.Name);
    }
}

[Log]
public class LogThings : Task
{
    // outputs "LogThings started." before Execute starts
    public override void Execute()
    {
        Console.WriteLine("{0} executing.", Name); // outputs "LogThings executing."
    }
    // outputs "LogThings finished." after Execute finishes
}
