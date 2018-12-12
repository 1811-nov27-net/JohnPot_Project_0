using System;
using Xunit;
using PizzaStoreLibrary.library;
using System.Collections.Generic;
using System.Linq;

namespace PizzaStoreTesting.test
{

    // Test cases for user object model
    public class UserTesting
    {

        #region User Name
        /***** has first name, last name, etc. *****/

        [Theory]
        // Test user with provided full name
        [InlineData(new string[] { "John", "Pot" },
                    new string[] { "John", "Pot" })]
        // User only provides first name
        [InlineData(new string[] { "John" },
                    new string[] { Utilities.InvalidName, Utilities.InvalidName })]
        // Empty name
        [InlineData(new string[] { "" },
                    new string[] { Utilities.InvalidName, Utilities.InvalidName })]
        // No name provided
        [InlineData(new string[] { },
                    new string[] { Utilities.InvalidName, Utilities.InvalidName })]
        // Too many names
        [InlineData(new string[] { "John", "Pot", "Fred", "Jim", "Bob" },
                    new string[] { "John", "Bob" })]
        public void UserHasName(string[] fullName, string[] expected)
        {
            // Arrange
            User user = new User(fullName);

            // Act

            // Assert
            Assert.Equal(expected, (string[])(user.FullName));
        }
        #endregion

        #region Default location
        /***** has a default location to order from *****/

        [Theory]
        // Pass only one location
        [InlineData(new string[] { "John", "Pot" },
                    "John's Pizzaria", "John's Pizzaria")]
        // TODO: Implement location validation
        //// Invalid location
        //[InlineData(new string[] { "John", "Pot" },
        //            "Free Pizzaria", Name.InvalidLocation)]
        // No location provided
        [InlineData(new string[] { "John", "Pot" },
                    null, Utilities.InvalidLocation)]
        // Pass in empty location name
        [InlineData(new string[] { "John", "Pot" },
                    "", Utilities.InvalidLocation)]
        public void UserHasDefaultLocationToOrderFromUsingConstructors(string[] fullName, string location, string expected)
        {
            // Arrange
            User user = new User(fullName, location);

            // Assert
            Assert.Equal(expected, user.DefaultLocation);

        }

        [Theory]
        // Pass valid locations
        [InlineData(new string[] { "John", "Pot" },
                    "John's Pizzaria", 
                    "Howie's Pizzaria", 
                    "Howie's Pizzaria")]
        // Pass empty default location to set
        //  In this case, we're going to revert the default location
        //   to the location that's already on record
        [InlineData(new string[] { "John", "Pot" },
                    "John's Pizzaria",
                    "",
                    "John's Pizzaria")]
        // No locations provided at all
        [InlineData(new string[] { "John", "Pot" },
                    "",
                    "",
                    Utilities.InvalidLocation)]
        // Given no initial location but later passed 
        //  a default location
        [InlineData(new string[] { "John", "Pot" },
                    null,
                    "John's Pizzaria",
                    "John's Pizzaria")]
        // TODO: Validate locations exist? 
        //// Use invalid location with location on file
        //[InlineData(new string[] { "John", "Pot" },
        //            "John's Pizzaria",
        //            "Free Pizzaria",
        //            "John's Pizzaria")]
        // TODO: Validate locations exist?
        //// Use invalid location without location on file
        //[InlineData(new string[] { "John", "Pot" },
        //            null,
        //            "Free Pizzaria",
        //            Name.InvalidLocation)]
        public void UserHasDefaultLocationToOrderFromAfterSettingDefault(
            string[] fullName, 
            string location, 
            string defaultLocation,
            string expected)
        {
            // Arrange
            User user = new User(fullName, location);

            // Act
            user.SetDefaultLocation(defaultLocation);

            // Assert
            Assert.Equal(expected, user.DefaultLocation);

        }
        #endregion

        /***** cannot place more than one order from the same location within two hours *****/

        [Theory]
        // User places order of Cheese + Pepperoni pizza successfully
        [InlineData(true,
                    new string[] { "Cheese", "Pepperoni" })]
        // Try to order 0 pizzas
        [InlineData(false,
                    new string[] { "" })]
        // Try to order too many pizzas (13)
        [InlineData(true,
                    new string[] { "Cheese" },
                    new string[] { "Cheese" },
                    new string[] { "Cheese" },
                    new string[] { "Cheese" },
                    new string[] { "Cheese" },
                    new string[] { "Cheese" },
                    new string[] { "Cheese" },
                    new string[] { "Cheese" },
                    new string[] { "Cheese" },
                    new string[] { "Cheese" },
                    new string[] { "Cheese" },
                    new string[] { "Cheese" },
                    new string[] { "Cheese" })]
        public void UserCanPlaceOrders(bool expected, params string[][] pizzas)
        {
            // Arrange
            User user = new User("John", "Pot");
            Location location = new Location("John's Pizzaria");
            location.StockInventory(new KeyValuePair<string, int>("Cheese", 100));
            location.StockInventory(new KeyValuePair<string, int>("Pepperoni", 100));
            Order order1 = new Order(user);
            foreach (string[] pizza in pizzas)
            {
                order1.AddPizzaToOrder(pizza);
            }


            // Act
            int orderPlaced = location.PlaceOrder(order1);

            // Assert
            Assert.True((orderPlaced>0) == expected);
        }


        [Fact]
        public void UserCannotOrderFromSameLocationWithinTwoHours()
        {
            // Arrange
            User user = new User("John", "Pot");

            // For each order create a new location
            Location location1 = new Location("John's Pizzaria");
            location1.StockInventory(new KeyValuePair<string, int>("Cheese", 100));
            location1.StockInventory(new KeyValuePair<string, int>("Pepperoni", 100));
            // Second location since we can't order from the same one
            //  but can order from two different ones in two hours
            Location location2 = new Location("John's Second Pizzaria");
            location2.StockInventory(new KeyValuePair<string, int>("Cheese", 100));
            location2.StockInventory(new KeyValuePair<string, int>("Pepperoni", 100));
            // Create a couple different orders
            Order order1 = new Order(user);
            order1.AddPizzaToOrder("Cheese");
            Order order2 = new Order(user);
            order2.AddPizzaToOrder("Pepperoni");

            // Act
            // Placing first order at first location should be successful
            int firstOrder = location1.PlaceOrder(order1);
            // A second (different) order should be rejected
            int SecondOrder = location1.PlaceOrder(order2);
            // Placing an order at a new location should be successful
            int ThirdOrder = location2.PlaceOrder(order1);
            // Just for completeness lets test placing another
            //  order at the second location before enough time has
            //  elapsed
            int FourthOrder = location2.PlaceOrder(order2);

            // Assert
            Assert.True(firstOrder>0);
            Assert.False(SecondOrder>0);
            Assert.True(ThirdOrder>0);
            Assert.False(FourthOrder>0);


        }
        [Fact]
        public void UserCanPlaceSecondOrderAfterTwoHoursHaveElapsed()
        {
            // Arrange
            User user = new User("John", "Pot");

            // For each order create a new location
            Location location1 = new Location("John's Pizzaria");
            location1.StockInventory(new KeyValuePair<string, int>("Cheese", 100));
            location1.StockInventory(new KeyValuePair<string, int>("Pepperoni", 100));
            // Create a couple different orders
            Order order1 = new Order(user);
            order1.AddPizzaToOrder("Cheese");
            Order order2 = new Order(user);
            order2.AddPizzaToOrder("Pepperoni");

            // Act
            // Placing first order at first location should be successful
            int firstOrder = location1.PlaceOrder(order1);
            // Act like the order was placed 3 hours ago...
            TimeSpan threeHours = new TimeSpan(3, 0, 0);
            order1.TimePlaced -= threeHours;
            // Place a new order after three hours. Should succeed
            int secondOrder = location1.PlaceOrder(order2);

            // Assert
            Assert.True(firstOrder>0);
            Assert.True(secondOrder>0);


        }


    }

}