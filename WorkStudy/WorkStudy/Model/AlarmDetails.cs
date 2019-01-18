﻿using SQLite;

namespace WorkStudy.Model
{
    [Table("AlarmDetails")]
    public class AlarmDetails : BaseEntity
    {
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public int Interval { get; set; }
        public int StudyId { get; set; }
    }
}
