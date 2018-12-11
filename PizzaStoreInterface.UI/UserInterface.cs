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
            User John1 = new User("John1", "Pot");
            Location l = new Location("John's Pizzaria");
            l.StockInventory(new KeyValuePair<string, int>("Cheese", 100));
            l.StockInventory(new KeyValuePair<string, int>("Pepperoni", 100));
            l.StockInventory(new KeyValuePair<string, int>("Olives", 100));
            l.StockInventory(new KeyValuePair<string, int>("Pineapple", 100));

            StoreManager store = new StoreManager();
            store.RegisterNewLocation(l);
            store.RegisterNewUser(John1);

            // Place a bunch of orders for John1
            for(int i = 0; i < 10; ++i)
            {
                Order bulkOrder = new Order(John1, l);
                bulkOrder.AddPizzaToOrder("Cheese");
                bool orderPlaced = store.PlaceOrder(bulkOrder);
                bulkOrder.TimePlaced -= new TimeSpan(3 + i, 0, 0);
            }
            // Add a couple of different pizzas in the mix...
            Order o = new Order(John1, l);
            o.AddPizzaToOrder("Olives", "Pepperoni");
            o.AddPizzaToOrder("Olives", "Cheese");
            bool b = store.PlaceOrder(o);
            o.TimePlaced -= new TimeSpan(5, 0, 0);
            Order p = new Order(John1, l);
            p.AddPizzaToOrder("Pineapple", "Pepperoni");
            p.AddPizzaToOrder("Olives", "Cheese");
            p.AddPizzaToOrder("Olives", "Cheese");
            store.PlaceOrder(p);
            p.TimePlaced -= new TimeSpan(8, 0, 0);

            List<Order> userHistory = store.GetUserHistory(John1);

            Comparison<Order> sortMethod = delegate (Order o1, Order o2)
            {
                return o1.TimePlaced.CompareTo(o2.TimePlaced);
            };

            Console.WriteLine("***** Displaying order by earliest ******" );
            Console.WriteLine();
            store.DisplayOrderHistoryBy(userHistory, sortMethod);

            sortMethod = delegate (Order o1, Order o2)
            {
                return -o1.TimePlaced.TimeOfDay.CompareTo(-o2.TimePlaced.TimeOfDay);
            };

            Console.WriteLine("***** Displaying order by latest *****");
            Console.WriteLine();
            store.DisplayOrderHistoryBy(userHistory, sortMethod);

            sortMethod = delegate (Order o1, Order o2)
            {
                return o1.Cost.CompareTo(o2.Cost);
            };

            Console.WriteLine("***** Displaying order by cheapest *****");
            Console.WriteLine();
            store.DisplayOrderHistoryBy(userHistory, sortMethod);

            sortMethod = delegate (Order o1, Order o2)
            {
                return -o1.Cost.CompareTo(-o2.Cost);
            };

            Console.WriteLine("***** Displaying order by most expensive *****");
            Console.WriteLine();
            store.DisplayOrderHistoryBy(userHistory, sortMethod);

            string st = "PizzaId";
            Console.WriteLine(st + 1);
            Console.WriteLine(st);
        }
    }
}