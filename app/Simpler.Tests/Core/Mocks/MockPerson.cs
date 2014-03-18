namespace Simpler.Tests.Core.Mocks
{
    public class MockPerson
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public MockPet Pet { get; set; }
        public MockVehicle[] Vehicles { get; set; }
        public MockEnum MockEnum { get; set; }
        public dynamic Other { get; set; }
    }
}
