using System;
using System.IO;
using NUnit.Framework;
using Simpler;
using System.Linq;

[TestFixture]
public class PerformanceTest
{
    [Test]
    public void play_ball_should_take_under_a_second()
    {
        var playBall = Task.New<PlayBall>();
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            playBall.Execute();
        }
        CheckPerformance(playBall, maxSeconds: 1);
    }

    static void CheckPerformance(Task task, int maxSeconds)
    {
        var max = task.Stats.ExecuteDurations.Max(ed => ed.TotalSeconds);
        Assert.That(max, Is.LessThan(maxSeconds), "{0} took too long.", task.Name);
    }
}
