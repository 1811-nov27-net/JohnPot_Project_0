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
}
