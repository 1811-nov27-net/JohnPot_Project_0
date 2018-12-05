using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using PizzaStoreLibrary.library;

namespace PizzaStoreTesting.test
{
    public class LocationTesting
    {
        #region Inventory Has Ingredients
        // has an inventory of pizza ingredients

        [Theory]
        // Pass in basic inventory of 3 ingredient types at 10 each
        [InlineData(new string[] { "Cheese", "Pepperoni", "Olives" },
                    new int[] { 10, 10, 10 },
                    "John's Pizzaria",
                    30)]
        // Pass just one ingredient type
        [InlineData(new string[] { "Cheese" },
                    new int[] { 10 },
                    "John's Pizzaria",
                    10)]
        // No ingredients
        [InlineData(new string[] { },
                    new int[] { },
                    "John's Pizzaria",
                    0)]
        public void LocationHasIngredients(string[] ingredients, int[] inventory, string name, int totalInventory)
        {
            // Arrange
            Location location = new Location(name);
            for (int i = 0; i < ingredients.Length; ++i)
            {
                location.StockInventory(new KeyValuePair<string, int>(ingredients[i], inventory[i]));
            }

            // Act
            Dictionary<string, int> ingredientInventory = location.Inventory;
            int total = ingredientInventory.Sum(i => i.Value);


            // Assert
            Assert.Equal(totalInventory, total);

        }
        #endregion

        #region Orders Being Placed
        // inventory decreases when orders are accepted
        // rejects orders that cannot be fulfilled with remaining inventory

        [Theory]
        // Stock the inventory with 10 cheese / pepperoni 
        //   and create 1 pizza with cheese + pepperoni. 
        //   Should leave 9 cheese and 9 pepperoni
        [InlineData(new string[] { "Cheese", "Pepperoni" },
                    new int[] { 10, 10 },
                    "John's Pizzaria",
                    18, true,
                    new string[] { "Cheese", "Pepperoni" })]
        // Passing in multiple pizzas
        [InlineData(new string[] { "Cheese", "Pepperoni" },
                    new int[] { 10, 10 },
                    "John's Pizzaria",
                    14, true,
                    new string[] { "Cheese", "Pepperoni" },
                    new string[] { "Cheese", "Pepperoni" },
                    new string[] { "Cheese", "Pepperoni" })]
        // Insufficient ingredients for order
        [InlineData(new string[] { "Cheese", "Pepperoni" },
                    new int[] { 2, 2 },
                    "John's Pizzaria",
                    4, false,
                    new string[] { "Cheese", "Pepperoni" },
                    new string[] { "Cheese", "Pepperoni" },
                    new string[] { "Cheese", "Pepperoni" })]
        // No inventory at all
        [InlineData(new string[] { },
                    new int[] { },
                    "John's Pizzaria",
                    0, false,
                    new string[] { "Cheese", "Pepperoni" })]
        // Only enough of one ingredient
        [InlineData(new string[] { "Cheese" },
                    new int[] { 10 },
                    "John's Pizzaria",
                    10, false,
                    new string[] { "Cheese", "Pepperoni" })]
        // Inventory has ingredients but not the one requested
        [InlineData(new string[] { "Cheese", "Pepperoni" },
                    new int[] { 10, 10 },
                    "John's Pizzaria",
                    20, false,
                    new string[] { "Olives" })]
        // Ingredients/Inventory are corresponding; 
        //  ingredients[0] has inventory[0] available 
        public void InventoryDepletesAsOrdersAreProcessed(
            string[] ingredients, int[] inventory, string name,
            int expectedRemaing, bool expectedOrderPlaced,
            params string[][] pizzas)
        {
            // Arrange
            // Create and stock a new location
            Location location = new Location(name);
            for (int i = 0; i < ingredients.Length; ++i)
            {
                location.StockInventory(new KeyValuePair<string, int>(ingredients[i], inventory[i]));
            }

            // Create a new order to be placed
            Order order1 = new Order(new string[] { "John", "Pot" }, location);
            // Build and add pizzas to the order
            foreach (string[] pizza in pizzas)
            {
                Pizza p = new Pizza(pizza);
                order1.AddPizzaToOrder(p);
            }


            // Act
            bool orderSuccessfull = location.PlaceOrder(order1);
            int remainingInventory = location.Inventory.Sum(i => i.Value);

            // Assert
            Assert.Equal(expectedRemaing, remainingInventory);
            Assert.True(expectedOrderPlaced == orderSuccessfull);
        }

        #endregion

        #region Order History

        // has order history

        [Theory]
        // Retrieve first order placed
        [InlineData(0,
                    new string[] { "John", "Pot" },
                    new string[] { "Jim", "Fred" },
                    new string[] { "Jane", "Doe" })]
        // Retrieve middle order
        [InlineData(1,
                    new string[] { "John", "Pot" },
                    new string[] { "Jim", "Fred" },
                    new string[] { "Jane", "Doe" })]
        // Retrieve last order
        [InlineData(2,
                    new string[] { "John", "Pot" },
                    new string[] { "Jim", "Fred" },
                    new string[] { "Jane", "Doe" })]
        // Try to find a user that hasn't place
        //  an order
        [InlineData(-1,
                    new string[] { "John", "Pot" },
                    new string[] { "Jim", "Fred" },
                    new string[] { "Jane", "Doe" })]
        public void FindPastOrderByUser(
            int userToCheck, params string[][] users)
        {
            // Arrange
            // Create and stock a new location
            Location location = new Location("John's Pizzaria");
            location.StockInventory(new KeyValuePair<string, int>("Cheese", 10));
            location.StockInventory(new KeyValuePair<string, int>("Pepperoni", 10));
            // Create list of users
            List<User> userList = new List<User>();
            // Create order for each user to be placed
            List<Order> orderList = new List<Order>();
            foreach (string[] user in users)
            {
                userList.Add(new User(user));
                Order o = new Order(user);
                orderList.Add(o);
                location.PlaceOrder(o);
            }


            // Act
            Order pastOrder;
            if (userToCheck == -1)
                pastOrder = location.GetLastOrder(new User("Invalid", "User"));
            else
                pastOrder = location.GetLastOrder(userList[userToCheck]);

            // Assert
            if (pastOrder != null)
                Assert.True(pastOrder == orderList[userToCheck]);

            if (userToCheck == -1)
                Assert.True(pastOrder == null);

        }
        [Fact]
        public void FindPastOrderByTimePlaced()
        {
            // Arrange
            // Create and stock a new location
            Location location = new Location("John's Pizzaria");
            location.StockInventory(new KeyValuePair<string, int>("Cheese", 10));
            location.StockInventory(new KeyValuePair<string, int>("Pepperoni", 10));

            // Create an order
            User john = new User("John", "Pot");
            Order order1 = new Order(john);
            order1.AddPizzaToOrder(new Pizza("Cheese"));

            Order order2 = new Order("Jim", "Fred");
            order2.AddPizzaToOrder("Cheese");

            // Act
            bool orderPlaced = location.PlaceOrder(order1);
            Order oldOrder = null;
            if (orderPlaced)
            {
                oldOrder = location.GetLastOrder();
            }
            orderPlaced = location.PlaceOrder(order2);
            oldOrder = null;
            if (orderPlaced)
            {
                oldOrder = location.GetLastOrder();
            }

            // Assert
            Assert.True(oldOrder == order2);
        }

        [Theory]
        // Order 1 pizza
        [InlineData(true,
                    new string[] { "Cheese" })]
        // Order multiple pizzas
        [InlineData(true,
                    new string[] { "Cheese" },
                    new string[] { "Pepperoni" },
                    new string[] { "Olives" })]
        // Try to order 0 pizzas
        [InlineData(false,
                    new string[] { "" })]
        public void SuggestOrderBasedOnPreviousOrders(bool expected, params string[][] pizzas)
        {
            // Arrange
            Location location = new Location("John's Pizzaria");
            location.StockInventory(new KeyValuePair<string, int>("Cheese", 100));
            location.StockInventory(new KeyValuePair<string, int>("Pepperoni", 100));
            location.StockInventory(new KeyValuePair<string, int>("Olives", 100));
            User user = new User("John", "Pot");
            Order order1 = new Order(user);
            foreach (string[] pizza in pizzas)
            {
                order1.AddPizzaToOrder(pizza);
            }

            // Act
            bool orderPlaced = location.PlaceOrder(order1);
            Order order2 = location.SuggestOrder(user);
            

            // Assert
            if (order2 != null)
                Assert.True((order1.Equals(order2)) == expected);
            else
                Assert.True(orderPlaced == expected);

        }

        [Theory]
        // Passing in if the test should pass or fail and each 
        //  string[] will be a new order
        [InlineData(true, 
            new string[] { "Cheese"},
            new string[] { "Cheese"},
            new string[] { "Cheese"},
            new string[] { "Cheese"})]
        // Order no pizzas
        [InlineData(true,
            new string[] { "" })]
        // Place a bunch of different orders
        [InlineData(true,
            new string[] { "Cheese" },
            new string[] { "Pepperoni" },
            new string[] { "Olives" },
            new string[] { "Cheese", "Olives", "Pepperoni" })]
        public void GetFullHistoryOfLoctaion(bool expected, params string[][] orders)
        {
            // Arrange
            Location location = new Location("John's Pizzaria");
            location.StockInventory(new KeyValuePair<string, int>("Cheese", 100));
            location.StockInventory(new KeyValuePair<string, int>("Pepperoni", 100));
            location.StockInventory(new KeyValuePair<string, int>("Olives", 100));
            User user = new User("John", "Pot");
            List<Order> orderList = new List<Order>();
            foreach (string[] pizza in orders)
            {
                Order o = new Order(user);
                o.AddPizzaToOrder(pizza);
                orderList.Add(o);
            }
            bool orderPlaced = false;
            // Place all the orders
            foreach (Order o in orderList)
            {
                orderPlaced = location.PlaceOrder(o);
                // Manually modify the order time
                //  so we can place multiple
                if(orderPlaced)
                {
                    TimeSpan threeHours = new TimeSpan(3, 0, 0);
                    o.OrderTime -= threeHours;
                }
            }

            // Act

            Stack<Order> orderHistory = location.GetFullHistory();

            // Assert
            foreach (Order o1 in orderHistory)
            {
                bool found = false;
                foreach(Order o2 in orderList)
                {
                    if(o1.Equals(o2))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    Assert.True(expected == found);
            }

            Assert.True(expected == true);

        }

        [Theory]
        // Passing in if the test should pass or fail and each 
        //  string[] will be a new order
        [InlineData(true,
            new string[] { "Cheese" },
            new string[] { "Cheese" },
            new string[] { "Cheese" },
            new string[] { "Cheese" })]
        // Order no pizzas
        [InlineData(true,
            new string[] { "" })]
        // Place a bunch of different orders
        [InlineData(true,
            new string[] { "Cheese" },
            new string[] { "Pepperoni" },
            new string[] { "Olives" },
            new string[] { "Cheese", "Olives", "Pepperoni" })]
        public void GetFullHistoryOfSpecificUser(bool expected, params string[][] orders)
        {
            // Arrange
            Location location = new Location("John's Pizzaria");
            location.StockInventory(new KeyValuePair<string, int>("Cheese", 100));
            location.StockInventory(new KeyValuePair<string, int>("Pepperoni", 100));
            location.StockInventory(new KeyValuePair<string, int>("Olives", 100));
            User userToCheckHistoryOf = new User("John", "Pot");
            User userToAddAdditionalOrders = new User("Dummy", "dummy");
            List<Order> orderList = new List<Order>();
            foreach (string[] pizza in orders)
            {
                Order o = new Order(userToCheckHistoryOf);
                o.AddPizzaToOrder(pizza);
                orderList.Add(o);
            }
            bool orderPlaced = false;

            // Place all the orders for main user
            foreach (Order o in orderList)
            {
                orderPlaced = location.PlaceOrder(o);
                // Manually modify the order time
                //  so we can place multiple
                if (orderPlaced)
                {
                    TimeSpan threeHours = new TimeSpan(3, 0, 0);
                    o.OrderTime -= threeHours;
                }
            }
            // Place a dummy order to differentiate between
            //  full location history and just the user history
            Order d = new Order(userToAddAdditionalOrders);
            d.AddPizzaToOrder("Cheese");
            location.PlaceOrder(d);

            // Act

            Stack<Order> userOrderHistory = location.GetFullHistory(userToCheckHistoryOf);

            // Assert
            foreach (Order o1 in userOrderHistory)
            {
                bool found = false;
                foreach (Order o2 in orderList)
                {
                    if (o1.Equals(o2))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    Assert.True(expected == found);
            }

            Assert.True(expected == true);
        }
        
        #endregion
    }
}
