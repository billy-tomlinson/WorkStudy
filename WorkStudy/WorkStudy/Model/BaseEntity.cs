using System;
using SQLite;

namespace WorkStudy.Model
{
    public class BaseEntity
    {
        public BaseEntity()
        {

        }
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
