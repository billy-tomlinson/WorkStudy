using System;
using System.Drawing;
using SQLite;
using WorkStudy.Services;

namespace WorkStudy.Model
{
    public class BaseEntity : IDisposable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}


