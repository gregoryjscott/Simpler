using System;
using System.Data;
using System.Linq;
using Moq;
using NUnit.Framework;
using Simpler.Data.Tasks;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class BuildObjectsTest
    {
        [Test]
        public void should_return_an_object_for_each_record_returned_by_the_select_command()
        {
            var buildObjects = Execute.Now<BuildObjects<MockPerson>>(bo => {
                bo.In.Reader = ResultsTest.SetupJohnAndJane();
            });

            Assert.That(buildObjects.Out.Objects.Count(), Is.EqualTo(2));
            Assert.That(buildObjects.Out.Objects[0].Name, Is.EqualTo("John Doe"));
            Assert.That(buildObjects.Out.Objects[1].Name, Is.EqualTo("Jane Doe"));
        }

        [Test]
        public void should_build_typed_objects_if_given_strong_type()
        {
            var buildObjects = Execute.Now<BuildObjects<MockPerson>>(bo => {
                bo.In.Reader = ResultsTest.SetupJohnAndJane();
                bo.BuildTyped = Fake.Task<BuildTyped<MockPerson>>(bt => bt.Out.Object = new MockPerson());
                bo.BuildDynamic = Fake.Task<BuildDynamic>();
            });

            Assert.That(buildObjects.BuildTyped.Stats.ExecuteCount, Is.GreaterThan(0));
            Assert.That(buildObjects.BuildDynamic.Stats.ExecuteCount, Is.EqualTo(0));
        }

        [Test]
        public void should_build_dynamic_objects_if_given_dynamic_type()
        {
            var buildObjects = Execute.Now<BuildObjects<object>>(bo => {
                bo.In.Reader = ResultsTest.SetupJohnAndJane();
                bo.BuildTyped = Fake.Task<BuildTyped<dynamic>>();
                bo.BuildDynamic = Fake.Task<BuildDynamic>(bd => bd.Out.Object = new MockPerson());
            });

            Assert.That(buildObjects.BuildTyped.Stats.ExecuteCount, Is.EqualTo(0));
            Assert.That(buildObjects.BuildDynamic.Stats.ExecuteCount, Is.GreaterThan(0));
        }
    }
}
