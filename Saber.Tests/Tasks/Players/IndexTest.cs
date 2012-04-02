using System;
using NUnit.Framework;
using Saber.Tasks.Players;
using Simpler;

namespace Saber.Tests.Tasks.Players
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
            Test<Index>.Create()
                .Act()
                .Assert(t => Assert.That(t.Output.Players.Length, Is.GreaterThan(0)));
        }
    }
}
