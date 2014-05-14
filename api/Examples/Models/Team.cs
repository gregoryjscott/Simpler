namespace Examples.Models
{
    public class Team
    {
        public string League { get; set; }
        public string Division { get; set; }
        public string Name { get; set; }
        public long FirstSeason { get; set; }
        public long Age { get { return 2014 - FirstSeason; } }
        public long G { get; set; }
        public long W { get; set; }
        public long L { get; set; }
        public long Pennants { get; set; }
        public long WorldSeries { get; set; }
        public long Playoffs { get; set; }
        public long R { get; set; }
        public long AB { get; set; }
        public long H { get; set; }
        public long HR { get; set; }
        public double BA { get; set; }
        public long RA { get; set; }
        public double Era { get; set; }
    }
}
