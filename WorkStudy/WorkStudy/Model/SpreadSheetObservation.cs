using System;

namespace WorkStudy.Model
{
    public class SpreadSheetObservation
    {
        public string OperatorName { get; set; }

        public DateTime Date => DateTime.Now;

        public int Rating { get; set; }

        public string ActivityName { get; set; }

        public int ObservationNumber { get; set; }

        public int StudyId { get; set; }
    }
}
