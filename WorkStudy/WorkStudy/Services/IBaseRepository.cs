using System;
using System.Collections.Generic;
using WorkStudy.Model;

namespace WorkStudy.Services
{
    public interface IBaseRepository<T> : IDisposable
        where T : BaseEntity, new()
    {
        IEnumerable<T> GetItems();

        IEnumerable<T> GetAllWithChildren();

        T GetItem(int id);

        T GetWithChildren(int id);

        int GetItemsCount();

        int SaveItem(T item);

        int DeleteItem(T item);

        void UpdateWithChildren(T item);

        void InsertOrReplaceWithChildren(T item);

        void InsertAll(List<T> items);

        void CreateTable();

        void DropTable();

        void ExecuteSQLCommand(string sqlCommand);
    }
}
