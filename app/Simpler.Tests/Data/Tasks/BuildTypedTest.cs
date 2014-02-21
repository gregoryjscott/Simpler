using System.Data;
using Moq;
using NUnit.Framework;
using Simpler.Data.Tasks;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class BuildTypedTest
    {
        [Test]
        public void should_build_an_instance_of_given_type()
        {
            var buildTyped = Execute.Now<BuildTyped<MockPerson>>(bt => {
                var mockDataRecord = new Mock<IDataRecord>();
                bt.In.DataRecord = mockDataRecord.Object;
            });

            Assert.That(buildTyped.Out.Object, Is.InstanceOf(typeof (MockPerson)));
        }

        [Test]
        public void should_populate_typed_object_using_all_columns_in_the_data_record()
        {
            var buildTyped = Execute.Now<BuildTyped<MockPerson>>(bt => {
                bt.In.DataRecord = ResultsTest.SetupJohn();
            });

            Assert.That(buildTyped.Out.Object.Name, Is.EqualTo("John Doe"));
            Assert.That(buildTyped.Out.Object.Age, Is.EqualTo(21));
        }

        [Test]
        public void should_throw_exception_if_a_data_record_column_is_not_a_property_of_the_object_class()
        {
            Assert.Throws(typeof (CheckException), () => Execute.Now<BuildTyped<MockPerson>>(bt => {
                var mockDataRecord = new Mock<IDataRecord>();
                mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(1);
                mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("SomeOtherColumn");
                mockDataRecord.Setup(dataRecord => dataRecord["SomeOtherColumn"]).Returns("whatever");
                bt.In.DataRecord = mockDataRecord.Object;
            }));
        }

        [Test]
        public void should_allow_object_to_have_properties_that_dont_have_matching_columns_in_the_data_record()
        {
            var buildTyped = Execute.Now<BuildTyped<MockPerson>>(bt => {
                var mockDataRecord = new Mock<IDataRecord>();
                mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(1);
                mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
                mockDataRecord.Setup(dataRecord => dataRecord["Name"]).Returns("John Doe");
                bt.In.DataRecord = mockDataRecord.Object;
            });

            Assert.That(buildTyped.Out.Object.Name, Is.EqualTo("John Doe"));
            Assert.That(buildTyped.Out.Object.Age, Is.Null);
        }

        [Test]
        public void should_build_enum_properties()
        {
            var buildTyped = Execute.Now<BuildTyped<MockPerson>>(bt => {
                var mockDataRecord = new Mock<IDataRecord>();
                mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(1);
                mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("MockEnum");
                mockDataRecord.Setup(dataRecord => dataRecord["MockEnum"]).Returns("One");
                bt.In.DataRecord = mockDataRecord.Object;
            });

            Assert.That(buildTyped.Out.Object.MockEnum, Is.EqualTo(MockEnum.One));
        }
    }
}
