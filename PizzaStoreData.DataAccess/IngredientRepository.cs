using Microsoft.EntityFrameworkCore;
using PizzaStoreLibrary.library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PizzaStoreData.DataAccess
{
    public class IngredientRepository : IRepository<Ingredient>
    {
        private readonly PizzaStoreContext _db;
        private readonly DbContextOptions<PizzaStoreContext> _options;

        public IngredientRepository(DbContextOptions<PizzaStoreContext> options)
        {
            _options = options;
            _db = new PizzaStoreContext(options);
        }

        public void Create(Ingredient entity)
        {
            _db.Add(entity);
            _db.SaveChanges();
        }

        public void Delete(Ingredient entity)
        {
            _db.Remove(entity);
            _db.SaveChanges();
        }

        public Ingredient GetById(int id)
        {
            return _db.Ingredient.Find(id);
        }
        public Ingredient GetByName(string name)
        {
            return _db.Ingredient.FirstOrDefault(i => i.Name == name);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Update(Ingredient entity)
        {
            _db.Entry(_db.Ingredient
                .Find(entity.IngredientId))
                .CurrentValues.SetValues(entity);
            _db.SaveChanges();
        }
    }
}
