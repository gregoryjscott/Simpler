using NUnit.Framework;
using Simpler.Tests.Mocks;

namespace Simpler.Tests
{
    [TestFixture]
    public class MapperTest
    {
        [Test]
        public void should_map_source_string_to_target_string()
        {
            // Arrange
            const string source = "4";

            // Act
            var target = Mapper.Map<string>(source);

            // Assert
            Assert.That(target, Is.EqualTo("4"));
        }

        [Test]
        public void should_map_source_int_to_target_int()
        {
            // Arrange
            const int source = 4;

            // Act
            var target = Mapper.Map<int>(source);

            // Assert
            Assert.That(target, Is.EqualTo(4));
        }

        [Test]
        public void should_map_source_double_to_target_double()
        {
            // Arrange
            const double source = 4.12;

            // Act
            var target = Mapper.Map<double>(source);

            // Assert
            Assert.That(target, Is.EqualTo(4.12));
        }

        [Test]
        public void should_map_source_decimal_to_target_decimal()
        {
            // Arrange
            const decimal source = 4.12m;

            // Act
            var target = Mapper.Map<decimal>(source);

            // Assert
            Assert.That(target, Is.EqualTo(4.12m));
        }

        [Test]
        public void should_map_source_object_to_target_object()
        {
            // Arrange
            var source = new MockObject { Age = 4 };

            // Act
            var target = Mapper.Map<MockObject>(source);

            // Assert
            Assert.That(target.Age, Is.EqualTo(4));
        }

        [Test]
        public void should_map_source_anonymous_object_to_target_object()
        {
            // Arrange
            var source = new { Age = 4 };

            // Act
            var target = Mapper.Map<MockObject>(source);

            // Assert
            Assert.That(target.Age, Is.EqualTo(4));
        }
    }
}
