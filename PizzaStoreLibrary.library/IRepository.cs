using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaStoreLibrary.library
{
    public interface IRepository<T>
    {
        T GetById(int id);
        void Create(T entity);
        void Delete(T entity);
        void Update(T entity);
        void SaveChanges();
    }
    public interface IJunctionRepository<T1, T2, T3>
    {
        T1 GetById(int id);
        void Update(T2 entity1, T3 entity2);
        void Create(T2 entity1, T3 entity2);
        void Delete(T2 entity1, T3 entity2);
        void SaveChanges();
    }
}
