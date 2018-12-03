using PizzaStoreLibrary.library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzaStoreInterface.UI
{
    // interactive console application
    // only display-related code can be here
    // low-priority component, will be replaced when we move to project 1
    class UserInterface
    {
        static void Main(string[] args)
        {
            // Create a new order to be placed
            Order order1 = new Order(new string[] { "John", "Pot" }, new Location("John's Pizzaria"));
            string[][] pizzas = new string[][]
            {
                new string[]{ "Olives" }
            };

            // Build and add pizzas to the order
            foreach (string[] pizza in pizzas)
            {
                Pizza p = new Pizza(pizza);
                order1.AddPizzaToOrder(p);
            }

            Location location = new Location("John's Pizzaria");
            Dictionary<string, int> ingredientList = new Dictionary<string, int>()
            {
                {"Cheese", 10 },
                {"Pepperoni", 10 }
            };
            location.StockInventory(ingredientList);
            location.PlaceOrder(order1);

            Console.WriteLine(location.Inventory.Sum(i => i.Value));
        }
    }
}
