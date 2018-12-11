using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using db = PizzaStoreData.DataAccess;
using lib = PizzaStoreLibrary.library;

namespace PizzaStoreInterface.UI
{
    public class Repo
    {

        public db.IngredientRepository ingredientRepository;
        public db.InventoryRepository inventoryRepository;
        public db.LocationRepository locationRepository;
        public db.OrderRepository orderRepository;
        public db.PizzaRepository pizzaRepository;
        public db.UserRepository userRepository;

        public Repo(DbContextOptions<db.PizzaStoreContext> options)
        {
            ingredientRepository = new db.IngredientRepository(options);
            inventoryRepository = new db.InventoryRepository(options);
            locationRepository = new db.LocationRepository(options);
            orderRepository = new db.OrderRepository(options);
            pizzaRepository = new db.PizzaRepository(options);
            userRepository = new db.UserRepository(options);
        }
        public void SaveAll()
        {
            userRepository.SaveChanges();
        }
    }
}
