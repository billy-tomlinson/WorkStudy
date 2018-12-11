using System.Collections.Generic;
using System.Linq;
using SQLite;
using SQLiteNetExtensions.Extensions;
using WorkStudy.Model;

namespace WorkStudy.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        private static readonly object locker = new object();
        string dbPath;
        public BaseRepository(string dbPath = null)
        {
            try
            {
                this.dbPath = dbPath;
                DatabaseConnection = dbPath == null ? new SQLiteConnection(App.DatabasePath) : new SQLiteConnection(dbPath);
            }
            catch (SQLiteException ex)
            {
                DatabaseConnection = null;
                DatabaseConnection = dbPath == null ? new SQLiteConnection(App.DatabasePath) : new SQLiteConnection(dbPath);

            }
        }

        public IEnumerable<T> GetItems()
        {
            lock (locker)
            {
                return DatabaseConnection.Table<T>().ToList().OrderBy(x => x.Id);
            }
        }


        public IEnumerable<T> GetAllWithChildren()
        {
            lock (locker)
            {
                return DatabaseConnection.GetAllWithChildren<T>().OrderBy(x => x.Id);
            }
        }


        public T GetWithChildren(int id)
        {
            lock (locker)
            {
                return DatabaseConnection.GetWithChildren<T>(id);
            }
        }

        public int GetItemsCount()
        {
            lock (locker)
            {
                return DatabaseConnection.Table<T>().Count();
            }
        }

        public T GetItem(int id)
        {
            lock (locker)
            {
                return DatabaseConnection.Table<T>().FirstOrDefault(i => i.Id == id);
            }
        }

        public int SaveItem(T item)
        {
            lock (locker)
            {
                if (item.Id != 0)
                {
                    DatabaseConnection.Update(item);
                    return 0;
                }
                DatabaseConnection.Insert(item);
                return GetId();
            }
        }


        public int DeleteItem(T item)
        {
            lock (locker)
            {
                return DatabaseConnection.Delete(item);
            }
        }

        public void UpdateWithChildren(T item)
        {
            lock (locker)
            {
                DatabaseConnection.UpdateWithChildren(item);
            }
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public SQLiteConnection DatabaseConnection { get; set; }


        private int GetId()
        {
            return DatabaseConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
        }

        public void InsertOrReplaceWithChildren(T item)
        {
            lock (locker)
            {
                DatabaseConnection.InsertOrReplaceWithChildren(item);
            }
        }

        ~BaseRepository()
        {
            // Finalizer calls Dispose(false)
            Dispose(true);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DatabaseConnection = null;
               
            }        
        }
    }
}
