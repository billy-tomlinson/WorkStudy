using System;

namespace StopWatch
{
    public class LapTime
    {
        public string TotalElapsedTime { get; set; }

        public string IndividualLapTime { get; set; }

        public double TotalElapsedTimeDouble { get; set; }

        public double IndividualLapDouble { get; set; }

        public TimeSpan TotalElapsedTimeImperial { get; set; }

        public TimeSpan IndividualLapImperial { get; set; }

        public int Count { get; set; }
    }
}
