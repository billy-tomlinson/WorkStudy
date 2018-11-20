﻿using System.Collections.Generic;
using System.Linq;
using SQLite;
using SQLiteNetExtensions.Extensions;
using WorkStudy.Model;

namespace WorkStudy.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        private static readonly object locker = new object();

        public BaseRepository()
        {
            DatabaseConnection = new SQLiteConnection(App.DatabasePath);
            //DatabaseConnection.DropTable<T>();
        }

        public BaseRepository(string dbPath)
        {
            DatabaseConnection = new SQLiteConnection(dbPath);
            //DatabaseConnection.DropTable<T>();
            DatabaseConnection.CreateTable<T>();
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
                    return DatabaseConnection.Update(item);

                DatabaseConnection.Insert(item);
                return GetId();
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

        public SQLiteConnection DatabaseConnection { get; }


        private int GetId()
        {
            return DatabaseConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
        }

    }
}
