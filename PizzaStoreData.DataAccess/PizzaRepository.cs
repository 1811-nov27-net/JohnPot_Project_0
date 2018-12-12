using Microsoft.EntityFrameworkCore;
using lib = PizzaStoreLibrary.library;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaStoreData.DataAccess
{
    public class PizzaRepository : lib.IRepository<Pizza>
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
            _db.SaveChanges();
        }
        public void Create(lib.Pizza entity)
        {
            // Create dictionary of ingredients...
            Dictionary<string, int> ingredientList = new Dictionary<string, int>();
            foreach (string ingredient in entity.Ingredients)
            {
                if (ingredientList.ContainsKey(ingredient))
                    ingredientList[ingredient]++;
                else
                {
                    ingredientList.Add(ingredient, 1);
                }
            }
            foreach(var i in ingredientList)
            {
                Pizza dbPizza = new Pizza();
                dbPizza.Count = i.Value;
                dbPizza.IngredientId = Mapper.GetIngredientByName(i.Key, _options).IngredientId;
                dbPizza.PizzaId = entity.Id;
                Create(dbPizza);
            }
        }

        public void Delete(Pizza entity)
        {
            _db.Remove(entity);
            _db.SaveChanges();
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
            _db.SaveChanges();
        }
    }
}
