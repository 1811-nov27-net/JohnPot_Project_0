using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lib = PizzaStoreLibrary.library;


namespace PizzaStoreData.DataAccess
{
    public static class Mapper
    {
        #region Location Mapping
        public static lib.Location Map(Location location, DbContextOptions<PizzaStoreContext> options)
        {
            // Start by validating the location
            if (!ValidLocation(location, options))
                return null;

            lib.Location libLocation = new lib.Location(location.Name);
            libLocation.Id = location.LocationId;
            return libLocation;
        }
        public static Location Map(lib.Location location, DbContextOptions<PizzaStoreContext> options)
        {
            // Start by validating the location
            if (!ValidLocation(location, options))
                return null;

            Location dbLocation = new Location();
            dbLocation.Name = location.Name;
            dbLocation.LocationId = location.Id;
            return dbLocation;
        }
        public static bool ValidLocation(Location location, DbContextOptions<PizzaStoreContext> options)
        {
            // Validate that the location exists inside the db
            return GetLocationById(location.LocationId, options) != null;
            
        }
        public static bool ValidLocation(lib.Location location, DbContextOptions<PizzaStoreContext> options)
        {
            // Validate that the location exists inside the db
            return GetLocationById(location.Id, options) != null;
        }
        public static Location GetLocationById(int? id, DbContextOptions<PizzaStoreContext> options)
        {
            if (id == null)
                return null;

            using (var database = new PizzaStoreContext(options))
            {
                return database.Location.Find(id);
            }
        }
        public static int GetLocationIdByName(string name, DbContextOptions<PizzaStoreContext> options)
        {
            using (var database = new PizzaStoreContext(options))
            {
                Location location = database.Location.FirstOrDefault(l => l.Name == name);
                if (location != null)
                    return location.LocationId;
            }

            return -1;
        }
        #endregion

        #region User Mapping
        public static bool ValidUser(User dbUser, DbContextOptions<PizzaStoreContext> options)
        {
            return GetUserById(dbUser.UserId, options) != null;
        }
        public static bool ValidUser(lib.User libUser, DbContextOptions<PizzaStoreContext> options)
        {
            return GetUserById(libUser.Id, options) != null;
        }
        public static User GetUserById(int? id, DbContextOptions<PizzaStoreContext> options)
        {
            if (id == null)
                return null;
            using (var database = new PizzaStoreContext(options))
            {
                return database.User.Find(id);
            }

        }
        public static int GetUserIdByName(string FirstName, string LastName, 
            DbContextOptions<PizzaStoreContext> options)
        {
            using (var db = new PizzaStoreContext(options))
            {
                User dbUser = db.User.FirstOrDefault(u => u.FirstName == FirstName && u.LastName == LastName);
                if (dbUser != null)
                    return dbUser.UserId;
            }
            return -1;
        }

        public static lib.User Map(User user, DbContextOptions<PizzaStoreContext> options)
        {
            if (user == null || !ValidUser(user, options))
                return null;

            lib.User u = new lib.User(user.FirstName, user.LastName);
            if (user.DefaultLocation != null)
                u.SetDefaultLocation(user.DefaultLocation.Name);

            u.Id = user.UserId;
            return u;
        }
        public static User Map(lib.User user, DbContextOptions<PizzaStoreContext> options)
        {
            if (user == null || !ValidUser(user, options))
                return null;

            User u = new User();
            u.FirstName = user.FirstName;
            u.LastName = user.LastName;
            u.UserId = GetUserIdByName(user.FirstName, user.LastName, options);
            if (user.DefaultLocation != lib.Utilities.InvalidLocation)
            {
                lib.Location l = new lib.Location(user.DefaultLocation);
                l.Id = GetLocationIdByName(user.DefaultLocation, options);
                if (l.Id != -1)
                    u.DefaultLocation = Map(l, options);
            }

            return u;
        }
        #endregion

        #region Pizza Mapping

        public static bool ValidPizza(lib.Pizza libPizza, DbContextOptions<PizzaStoreContext> options)
        {
            if (libPizza == null || libPizza.Ingredients.Count <= 0)
                return false;

            // Ensure each ingredient on the pizza list is valid.
            foreach (string ingredient in libPizza.Ingredients)
            {
                if (GetIngredientByName(ingredient, options) == null)
                    return false;
            }

            return true;
        }
        public static bool ValidPizza(List<Pizza> dbPizza, DbContextOptions<PizzaStoreContext> options)
        {
            if (dbPizza == null || dbPizza.Count == 0)
                return false;
            
            // Make sure each ingredient is valid
            foreach(Pizza p in dbPizza)
            {
                if (GetIngredientById(p.IngredientId, options) == null)
                    return false;
            }
            return true;
        }

        public static List<Pizza> Map(lib.Pizza libPizza, DbContextOptions<PizzaStoreContext> options)
        {
            if (!ValidPizza(libPizza, options))
                return null;

            List<Pizza> dbPizzaList = new List<Pizza>();
            using (var database = new PizzaStoreContext(options))
            {
                dbPizzaList.AddRange(database.Pizza.Where(p => p.PizzaId == libPizza.Id));
                return dbPizzaList;
            }

        }

        public static lib.Pizza Map(List<Pizza> dbPizza, DbContextOptions<PizzaStoreContext> options)
        {
            if (!ValidPizza(dbPizza, options))
                return null;

            List<string> ingredientList = new List<string>();
            foreach (Pizza p in dbPizza)
            {
                ingredientList.Add(GetIngredientById(p.IngredientId, options).Name);
            }
            lib.Pizza libPizza = new lib.Pizza(ingredientList);
            libPizza.Id = dbPizza[0].PizzaId;

            return libPizza;
        }

        #endregion

        #region Order Mapping

        // Assumes location mapping works correctly
        public static lib.Order Map(List<Order> order, DbContextOptions<PizzaStoreContext> options)
        {
            if (order.Count == 0)
                return null;

            Location dbLocation = GetLocationById(order[0].LocationId, options);
            lib.Order resultOrder = new lib.Order(order[0].User.FirstName, order[0].User.LastName);
            resultOrder.Location = Map(dbLocation, options);
            resultOrder.Id = order[0].OrderId;
            resultOrder.User = Map(GetUserById(order[0].UserId, options), options);
            resultOrder.TimePlaced = order[0].TimePlaced;

            // Need to set all pizzas for the order
            using (var db = new PizzaStoreContext(options))
            {
                // Grab all pizzas from the matching orders
                foreach (Order dbOrder in order)
                {
                    List<Pizza> dbPizzaList = db.Pizza.Where(p => p.PizzaId == dbOrder.PizzaId).ToList();
                    // This will grab a bunch of Pizza objects each having an individual 
                    //  ingredient to create the pizza
                    // PId 1, IId 1
                    // PId 1, IId 2
                    // These two pizza objects compose a sigle pizza
                    //  in lib space with two different ingredients
                    // dbPizzaList will store a list of all pizza objects
                    //  required to create a lib pizza

                    // Create lib.Pizza object by combining the relevant dbPizzas
                    lib.Pizza libPizza = new lib.Pizza();
                    foreach (Pizza dbPizza in dbPizzaList)
                    {
                        Ingredient dbIngredient = GetIngredientById(dbPizza.IngredientId, options);
                        if (dbIngredient != null)
                            libPizza.AddIngredientsToPizza(Map(dbIngredient, options));
                    }
                    libPizza.Id = dbOrder.PizzaId;
                    // The lib pizza has now been reconstructed 
                    resultOrder.PizzaList.Add(libPizza);
                }

            }

            return resultOrder;
        }

        public static List<Order> Map(lib.Order order, DbContextOptions<PizzaStoreContext> options)
        {
            List<Pizza> dbPizzaList = new List<Pizza>();
            foreach (lib.Pizza p in order.PizzaList)
            {
                dbPizzaList.AddRange(Map(p, options));
            }

            List<Order> resultOrderList = new List<Order>();
            foreach (Pizza p in dbPizzaList)
            {
                Order o = new Order();
                o.LocationId = order.Location.Id;
                o.UserId = order.User.Id;
                o.OrderId = order.Id;
                o.PizzaId = p.PizzaId;
                o.TimePlaced = order.TimePlaced;
                resultOrderList.Add(o);
            }

            return resultOrderList;
        }
        #endregion

        #region Ingredient Mapping

        // Map ingredient from dbIngredient to string (lib Ingredients
        //  only exist as strings
        public static string Map(Ingredient ingredient, DbContextOptions<PizzaStoreContext> options)
        {
            // TODO: Validate that the ingredient exists inside the 
            //  database.
            return ingredient.Name;
        }
        // From string to dbIngredient
        public static Ingredient Map(string ingredient, DbContextOptions<PizzaStoreContext> options)
        {
            // Look up ingredient in db
            using (var db = new PizzaStoreContext(options))
            {
                Ingredient dbIngredient = db.Ingredient.FirstOrDefault(i => i.Name == ingredient);
                if (dbIngredient == null)
                    return null;

                return dbIngredient;
            }
        }
        public static Ingredient GetIngredientById(int id, DbContextOptions<PizzaStoreContext> options)
        {
            using (var db = new PizzaStoreContext(options))
            {
                return db.Ingredient.Find(id);
            }
        }
        public static Ingredient GetIngredientByName(string name, DbContextOptions<PizzaStoreContext> options)
        {
            using (var database = new PizzaStoreContext(options))
            {
                if(database.Ingredient.Count() == 0)
                    return null;

                return database.Ingredient.First(i => i.Name == name);
            }
        }

        #endregion

    }
}
