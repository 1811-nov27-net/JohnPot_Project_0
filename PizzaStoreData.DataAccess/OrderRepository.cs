using Microsoft.EntityFrameworkCore;
using lib = PizzaStoreLibrary.library;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaStoreData.DataAccess
{
    public class OrderRepository : lib.IRepository<Order>
    {
        private readonly PizzaStoreContext _db;
        private readonly DbContextOptions<PizzaStoreContext> _options;

        public OrderRepository(DbContextOptions<PizzaStoreContext> options)
        {
            _options = options;
            _db = new PizzaStoreContext(options);
        }
        public void Create(Order entity)
        {
            _db.Add(entity);
        }

        public void Delete(Order entity)
        {
            _db.Remove(entity);
        }

        public Order GetById(int id)
        {
            return _db.Order.Find(id);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Update(Order entity)
        {
            _db.Entry(_db.Order
                .Find(entity.OrderId))
                .CurrentValues.SetValues(entity);
        }
    }
}
