using System.Drawing;
using SQLite;
using WorkStudy.Services;

namespace WorkStudy.Model
{
    public class BaseEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

    }
}


