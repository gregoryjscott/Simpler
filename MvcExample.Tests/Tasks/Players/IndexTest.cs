using System;
using MvcExample.Tasks.Players;
using NUnit.Framework;
using Simpler;

namespace MvcExample.Tests.Tasks.Players
{
    [TestFixture]
    public class IndexTest
    {
        [SetUp]
        public void SetDataDirectoryForConnectionString()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory + @"\App_Data");
        }

        [Test]
        public void should_return_list_of_players()
        {
            Test<Index>.New()
                .Act()
                .Assert(t => Assert.That(t.Output.Players.Length, Is.GreaterThan(0)));
        }
    }
}
