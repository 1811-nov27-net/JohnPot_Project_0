using Microsoft.EntityFrameworkCore;
using PizzaStoreLibrary.library;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaStoreData.DataAccess
{
    public class PizzaRepository : IRepository<Pizza>
    {
        private readonly PizzaStoreContext _db;
        private readonly DbContextOptions<PizzaStoreContext> _options;

        public PizzaRepository(DbContextOptions<PizzaStoreContext> options)
        {
            _options = options;
            _db = new PizzaStoreContext(options);
        }

        public void Create(Pizza entity)
        {
            _db.Add(entity);
        }

        public void Delete(Pizza entity)
        {
            _db.Remove(entity);
        }

        public Pizza GetById(int id)
        {
            return _db.Pizza.Find(id);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Update(Pizza entity)
        {
            _db.Entry(_db.Pizza
                .Find(entity.PizzaId))
                .CurrentValues.SetValues(entity);
        }
    }
}
