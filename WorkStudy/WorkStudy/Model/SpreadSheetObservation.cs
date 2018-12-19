using System;

namespace WorkStudy.Model
{
    public class SpreadSheetObservation
    {
        public string Study { get; set; }
        public DateTime Date { get; set; }
        public string OperatorName { get; set; }
        public int ObservationNumber { get; set; }
        public string ActivityName { get; set; }
        public int Rating { get; set; }

    }
}
