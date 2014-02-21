using System.Collections.Generic;

namespace Simpler.Tests.Core.Mocks
{
    public class MockComplexPerson
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public MockPet Pet { get; set; }
        public MockVechile[] Vechiles { get; set; }
        public MockEnum MockEnum { get; set; }
    }

    public class MockPet
    {
        public string Name { get; set; }
        public int? Age { get; set; }
    }

    public class MockVechile
    {
        public string Make { get; set; }
        public string Model { get; set; }
    }
}
