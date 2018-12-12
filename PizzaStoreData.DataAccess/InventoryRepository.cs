using Microsoft.EntityFrameworkCore;
using PizzaStoreLibrary.library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PizzaStoreData.DataAccess
{
    public class InventoryRepository : IRepository<Inventory>
    {
        private readonly PizzaStoreContext _db;
        private readonly DbContextOptions<PizzaStoreContext> _options;

        public InventoryRepository(DbContextOptions<PizzaStoreContext> options)
        {
            _options = options;
            _db = new PizzaStoreContext(options);
        }

        public void Create(Inventory entity)
        {
            _db.Add(entity);
            _db.SaveChanges();
        }

        public void Delete(Inventory entity)
        {
            _db.Remove(entity);
            _db.SaveChanges();
        }

        public Inventory GetById(int id)
        {
            // This is a junction table. It should
            //  require two ids. Fix later.
            return null;

            //return _db.Inventory.Find(id);
        }
        public Inventory GetById(int locationId, int ingredientId)
        {
            return _db.Inventory.FirstOrDefault(inv => 
            inv.LocationId == locationId 
            && inv.IngredientId == ingredientId);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Update(Inventory entity)
        {
            _db.Entry(_db.Inventory
                .Find(entity.LocationId, entity.IngredientId))
                .CurrentValues.SetValues(entity);
            _db.SaveChanges();
        }
    }
}
