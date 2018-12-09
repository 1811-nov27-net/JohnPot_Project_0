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
        private static PizzaStoreContext db;

        static Mapper()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PizzaStoreContext>();
            optionsBuilder.UseSqlServer(SecretConfiguration.ConnectionString);
            var options = optionsBuilder.Options;

            db = new PizzaStoreContext(options);
        }

        public static lib.Location Map(Location location)
            => new lib.Location(location.Name)
            {
                Id = location.LocationId
            };

        public static Location Map(lib.Location location)
            => new Location
            {
                Name = location.Name,
                LocationId = location.Id
            };

        public static lib.Pizza Map(Pizza pizza)
        {
            lib.Pizza p = new lib.Pizza();
            p.Id = pizza.PizzaId;
            string ingredientId = "IngredientId";
            for (int i = 1; i <= 8; ++i)
            {
                Ingredient ingredient = db.Ingredient.Find(ingredientId + i);
                p.AddIngredientsToPizza(ingredient.Name);
            }

            return p;
        }
        public static Pizza Map(lib.Pizza pizza)
        {
            Pizza p = new Pizza();
            p.PizzaId = pizza.Id;
            for(int i = 1; i <= 8; ++i)
            {
                Ingredient ingredient = db.Ingredient.FirstOrDefault(ing => ing.Name == pizza.Ingredients[i]);
            }

            return p;
        }
        public static lib.User Map(User user)
        {
            lib.User u = new lib.User(user.FirstName, user.LastName);
            if(user.DefaultLocation != null)
                u.SetDefaultLocation(user.DefaultLocation.Name);

            u.Id = user.UserId;
            return u;
        }
        public static User Map(lib.User user)
        {
            User u = new User();
            u.FirstName = user.FirstName;
            u.LastName = user.LastName;
            u.UserId = user.Id;
            if (user.DefaultLocation != null)
                u.DefaultLocation = Map(new lib.Location(user.DefaultLocation));

            return u;
        }

        public static lib.Order Map(Order order)
        {
            lib.Order o = new lib.Order(order.User.FirstName, order.User.LastName);
            o.Location = Map(order.Location);
            o.Id = order.OrderId;
            o.OrderTime = new TimeSpan(order.TimePlaced.Hour, order.TimePlaced.Minute, order.TimePlaced.Second);

            // User reflection to grab each pizza in context order by
            //  string name
            string pizzaId = "PizzaId";
            for (int i = 1; i <= 12; ++i)
            {
                int id = (int)order.GetType().GetProperty(pizzaId + i).GetValue(order);
                Pizza pizza = db.Pizza.FirstOrDefault(p => p.PizzaId == id);
                o.AddPizzaToOrder(Map(pizza));
            }

            return o;
        }
        public static Order Map(lib.Order order)
        {
            Order o = new Order();
            o.OrderId = order.Id;
            o.LocationId = Map(order.Location).LocationId;
            o.Location = Map(order.Location);
            // Bad. Assuming day is Dec 1, 2018 to convert
            //  from TimeSpan to DateTime. 
            // TODO: Change lib.Order.OrderTime to DateTime
            o.TimePlaced = new DateTime(2018, 12, 1, order.OrderTime.Hours, order.OrderTime.Minutes, order.OrderTime.Seconds);
            o.User = Map(order.User);
            o.UserId = order.User.Id;

            string id = "PizzaId";
            for (int i = 1; i < 12; ++i)
            {
                o.GetType().GetProperty(id + i).SetValue(order, order.PizzaList[i].Id);
            }

            return o;
        }


    }
}
