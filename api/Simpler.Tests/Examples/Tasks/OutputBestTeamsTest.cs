using System;
using System.Collections.Generic;
using System.Linq;
using Examples;
using Examples.Tasks;
using NUnit.Framework;
using Simpler;
using Examples.Models;

namespace Tests.Examples.Tasks
{
    [TestFixture]
    public class OutputBestTeamsTest
    {
        [SetUp]
        public void DisableLogging() { LogAttribute.Enabled = false; }

        [Test]
        public void outputs_best_teams_in_each_division()
        {
            var stats = new List<Stat>();
            var storeStats = Fake.Task<OutputStat>(os => stats.Add(os.In.Stat));

            var outputsBestTeams = Task.New<OutputBestTeams>();
            outputsBestTeams.OutputStat = storeStats;
            outputsBestTeams.Execute();

            var questions = String.Join("|", stats.Select(s => s.Question));
            Assert.That(questions.Contains("American League East"));
            Assert.That(questions.Contains("American League Central"));
            Assert.That(questions.Contains("American League West"));
            Assert.That(questions.Contains("National League East"));
            Assert.That(questions.Contains("National League Central"));
            Assert.That(questions.Contains("National League West"));
        }

        [Test]
        public void runs_under_1_second()
        {
            var skipOutput = Fake.Task<OutputStat>();

            var outputsBestTeams = Task.New<OutputBestTeams>();
            outputsBestTeams.OutputStat = skipOutput;
            outputsBestTeams.Execute();

            var seconds = outputsBestTeams.Stats.ExecuteDurations.Max(ed => ed.TotalSeconds);
            Assert.That(seconds, Is.LessThan(1));
        }
    }
}
