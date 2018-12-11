using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lib = PizzaStoreLibrary.library;
using Microsoft.EntityFrameworkCore;

namespace PizzaStoreData.DataAccess
{
    public class UserRepository : lib.IRepository<User>
    {
        private readonly PizzaStoreContext _db;
        private readonly DbContextOptions<PizzaStoreContext> _options;

        public UserRepository(DbContextOptions<PizzaStoreContext> options)
        {
            _options = options;
            _db = new PizzaStoreContext(options);
        }

        public void Create(User entity)
        {
            _db.Add(entity);
        }
        public void Create(lib.User entity)
        {
            User dbUser = new User
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName
            };
            _db.Add(dbUser);
        }

        public void Delete(User entity)
        {
            _db.Remove(entity);
        }

        public User GetById(int id)
        {
            return _db.User.Find(id);
        }
        public User GetByName(string firstName, string lastName)
        {
            return _db.User.FirstOrDefault(u => u.FirstName == firstName && u.LastName == lastName);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Update(User entity)
        {
            _db.Entry(_db.User
                .Find(entity.UserId))
                .CurrentValues.SetValues(entity);
        }
    }
}
