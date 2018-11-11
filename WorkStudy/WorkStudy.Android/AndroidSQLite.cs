
using SQLite;
using LocalDataAccess.Droid;
using System.IO;
using WorkStudy;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidSQLite))]
namespace LocalDataAccess.Droid
{
    public class AndroidSQLite : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var dbName = "ActivitySample.db";
            var path = Path.Combine(System.Environment.
              GetFolderPath(System.Environment.
              SpecialFolder.Personal), dbName);
            return new SQLiteConnection(path);
        }
    }
}