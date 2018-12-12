using Microsoft.EntityFrameworkCore;
using lib = PizzaStoreLibrary.library;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PizzaStoreData.DataAccess
{
    public class LocationRepository : lib.IRepository<Location>
    {
        private readonly PizzaStoreContext _db;
        private readonly DbContextOptions<PizzaStoreContext> _options;

        public LocationRepository(DbContextOptions<PizzaStoreContext> options)
        {
            _options = options;
            _db = new PizzaStoreContext(options);
        }

        public void Create(Location entity)
        {
            _db.Add(entity);
            _db.SaveChanges();
        }
        public void Create(lib.Location entity)
        {
            Location dbLocation = new Location();
            dbLocation.Name = entity.Name;
            Create(dbLocation);
            entity.Id = dbLocation.LocationId;
        }

        public void Delete(Location entity)
        {
            _db.Remove(entity);
            _db.SaveChanges();
        }
        public void Delete(lib.Location entity)
        {
            _db.Remove(GetById(entity.Id));
            _db.SaveChanges();
        }

        public Location GetById(int id)
        {
            return _db.Location.Find(id);
        }
        public Location GetByName(string name)
        {
            return _db.Location.FirstOrDefault(l => l.Name == name);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Update(Location entity)
        {
            _db.Entry(_db.Location
                .Find(entity.LocationId))
                .CurrentValues.SetValues(entity);

            _db.SaveChanges();
        }
        public void Update(lib.Location entity)
        {
            _db.Entry(_db.Location
                .Find(entity.Id))
                .CurrentValues.SetValues(Mapper.Map(entity, _options));
            _db.SaveChanges();
        }
    }
}
