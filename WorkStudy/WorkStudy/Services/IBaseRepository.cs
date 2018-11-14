using System;
using System.Collections.Generic;
using SQLite;
using WorkStudy.Model;

namespace WorkStudy.Services
{
    public interface IBaseRepository<T> : IDisposable
        where T : BaseEntity, new()
    {
        List<T> GetItems();

        T GetItem(int id);

        int GetItemsCount();

        int SaveItem(T item);

        SQLiteConnection DatabaseConnection { get; }

    }
}
