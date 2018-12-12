using Microsoft.EntityFrameworkCore;
using lib = PizzaStoreLibrary.library;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
            _db.SaveChanges();
        }
        public void Create(lib.Order entity)
        {
            foreach(lib.Pizza p in entity.PizzaList)
            {
                Order dbOrder = new Order();
                dbOrder.LocationId = entity.Location.Id;
                dbOrder.UserId = entity.User.Id;
                dbOrder.TimePlaced = entity.TimePlaced;
                dbOrder.PizzaId = p.Id;
                dbOrder.OrderId = entity.Id;
                Create(dbOrder);
                entity.Id = dbOrder.OrderId;
            }
        }

        public void Delete(Order entity)
        {
            _db.Remove(entity);
            _db.SaveChanges();
        }

        // Orders exist as list in the db
        public Order GetById(int id)
        {
            return null;
            return _db.Order.Find(id);
        }
        public List<Order> GetByLocationId(int id)
        {
            return _db.Order.Where(o => o.UserId == id).ToList();
        }
        public List<Order> GetAllOrders()
        {
            return _db.Order.ToList();
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
            _db.SaveChanges();
        }
    }
}
