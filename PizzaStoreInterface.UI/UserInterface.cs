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
            string[][] pizzas = new string[][]
            {
                new string[] { "Cheese" },
                new string[] { "Pepperoni" },
                new string[] { "Olives" },
            };

            // Arrange
            User user = new User("John", "Pot");
            Location location = new Location("John's Pizzaria");
            location.StockInventory(new KeyValuePair<string, int>("Cheese", 10));
            location.StockInventory(new KeyValuePair<string, int>("Pepperoni", 10));
            location.StockInventory(new KeyValuePair<string, int>("Olives", 10));
            Order order1 = new Order(user);
            foreach (string[] pizza in pizzas)
            {
                order1.AddPizzaToOrder(pizza);
            }

            location.PlaceOrder(order1);
            Order order2 = location.SuggestOrder(user);

            if(order1 == order2)
            {
                Console.WriteLine("Equals");
            }
            if(order1.Equals(order2))
            {
                Console.WriteLine("Equals");
            }

            int a;
        }
    }
}
