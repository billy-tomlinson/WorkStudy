
using SQLite;
namespace WorkStudy
{
    public interface ISQLite  
    {  
        SQLiteConnection GetConnection();  
    }  
}
