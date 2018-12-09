using PizzaStoreLibrary.library;
using System;
using System.Collections.Generic;
using Xunit;

namespace PizzaStoreTesting.test
{
    public class OrderTesting
    {
        #region Order Location
        // has a location
        [Theory]
        // Valid user and location given
        [InlineData("John's Pizzaria",
                    new string[] { "John", "Pot" },
                    "John's Pizzaria")]
        // Empty location string provided
        [InlineData("",
                    new string[] { "John", "Pot" },
                    Utilities.InvalidLocation)]
        // Null location
        [InlineData(null,
                    new string[] { "John", "Pot" },
                    Utilities.InvalidLocation)]
        public void OrderHasLocationAfterCreation(
            string location,
            string[] user,
            string expected)
        {
            // Arrange

            // Order can be created using strings or the classes
            //  provided

            // Order created using strings
            Order order1 = new Order(user, location);
            // Order created using Location class
            Location l = new Location(location);
            Order order2 = new Order(user, location);


            // Act
            string actualLocationOrder1 = order1.Location.Name;
            string actualLocationOrder2 = order2.Location.Name;

            // Assert
            Assert.Equal(expected, actualLocationOrder1);
            //Assert.Equal(expected, actualLocationOrder2.Name.Location);

        }
        #endregion

        #region Order User
        // has a user

        [Theory]
        // Passing in valid user data
        [InlineData(new string[] { "John", "Pot" },
                    new string[] { "John", "Pot" })]
        // Pass in first name only
        [InlineData(new string[] { "John" },
                    new string[] { "Invalid Name", "Invalid Name" })]
        // Pass in too many names
        [InlineData(new string[] { "John", "Thomas", "Pot" },
                    new string[] { "John", "Pot" })]
        // Empty name
        [InlineData(new string[] { "" },
                    new string[] { "Invalid Name", "Invalid Name" })]
        // Null name
        [InlineData(new string[] { null },
                    new string[] { "Invalid Name", "Invalid Name" })]
        // No name at all
        [InlineData(new string[] { },
                    new string[] { "Invalid Name", "Invalid Name" })]
        public void OrderHasUserAfterCreation(string[] user, string[] expected)
        {
            // Arrange

            // Create order using strings
            Order order1 = new Order(user);
            // Create order using User class
            User u = new User(user);
            Order order2 = new Order(u);

            // Act
            string[] actualUserNameOrder1 = order1.User.FullName;
            string[] actualUserNameOrder2 = order2.User.FullName;

            // Assert
            Assert.Equal(expected, actualUserNameOrder1);
            Assert.Equal(expected, actualUserNameOrder2);

        }

        #endregion

        #region Order Time
        // has an order time(when the order was placed)

        [Theory]
        [InlineData(true,
                    new string[] { "Cheese" })]
        public void OrderHasCorrectTimeWhenPlaced(bool expectedTimeIsNow, params string[][] pizzas)
        {
            // Arrange
            Location location = new Location("John's Pizzaria");
            location.StockInventory(new KeyValuePair<string, int>("Cheese", 100));
            User user = new User("John", "Pot");
            Order order1 = new Order(user);
            foreach (string[] pizza in pizzas)
            {
                order1.AddPizzaToOrder(pizza);
            }

            // Act
            bool orderPlaced = location.PlaceOrder(order1);


            // Assert
            if(orderPlaced)
            {
                // TODO: Figure out some TimeSpan epsilon or something
                //  to properly test this.
                //TimeSpan currentTime = DateTime.Now.TimeOfDay;
                //Assert.True((currentTime - order1.OrderTime) == TimeSpan.Zero 
                //            == expectedTimeIsNow);
            }

        }

        #endregion

        #region Number of Pizzas order can have
        // can have at most 12 pizzas

        [Theory]
        // Providing valid information. A user and 3 pizzas
        [InlineData(new string[] { "John", "Pot" },
                    3,
                    new string[] { "Pepperoni", "Olives" },
                    new string[] { "Cheese", "Olives" },
                    new string[] { "Olives" })]
        // Passing in only one pizza
        [InlineData(new string[] { "John", "Pot" },
                    1,
                    new string[] { "Pepperoni", "Olives" })]
        // Too many pizzas (13 total)
        [InlineData(new string[] { "John", "Pot" },
                    12,
                    new string[] { "Pepperoni", "Olives" },
                    new string[] { "Pepperoni", "Olives" },
                    new string[] { "Pepperoni", "Olives" },
                    new string[] { "Pepperoni", "Olives" },
                    new string[] { "Pepperoni", "Olives" },
                    new string[] { "Pepperoni", "Olives" },
                    new string[] { "Pepperoni", "Olives" },
                    new string[] { "Pepperoni", "Olives" },
                    new string[] { "Pepperoni", "Olives" },
                    new string[] { "Pepperoni", "Olives" },
                    new string[] { "Pepperoni", "Olives" },
                    new string[] { "Pepperoni", "Olives" },
                    new string[] { "Pepperoni", "Olives" })]
        // Passing in bad pizzas
        [InlineData(new string[] { "John", "Pot" },
                    0,
                    new string[] { null })]
        [InlineData(new string[] { "John", "Pot" },
                    0,
                    new string[] { "" })]
        [InlineData(new string[] { "John", "Pot" },
                    0,
                    new string[] { "Some bad topping" })]
        void AddingPizzaToAnOrderIncreasesPizzaOrderCount(string[] user, int expected, params string[][] pizzas)
        {
            // Arrange
            Order order1 = new Order(user);
            if (pizzas != null)
            {
                foreach (string[] pizza in pizzas)
                {
                    order1.AddPizzaToOrder(pizza);
                }
            }

            // Act
            int actualNumberOfPizzas = order1.PizzaList.Count;

            // Assert
            Assert.Equal(expected, actualNumberOfPizzas);

        }

        #endregion

        #region Order Cost


        [Theory]
        // One 1 topping pizza
        [InlineData(new string[] { "John", "Pot" },
                    11.00f,
                    new string[] { "Pepperoni" })]
        // One 2 topping pizza
        [InlineData(new string[] { "John", "Pot" },
                    12.00f,
                    new string[] { "Pepperoni", "Olives" })]
        // Testing two 1 topping pizzas
        [InlineData(new string[] { "John", "Pot" },
                    22.00f,
                    new string[] { "Pepperoni" },
                    new string[] { "Olives" })]
        // One 1 topping and one 2 topping
        [InlineData(new string[] { "John", "Pot" },
                    23.00f,
                    new string[] { "Pepperoni" },
                    new string[] { "Olives", "Pepperoni" })]
        // One cheese pizza (Contains decimals => 0.50)
        [InlineData(new string[] { "John", "Pot" },
                    10.50f,
                    new string[] { "Cheese" })]
        // One broken pizza and one valid one
        [InlineData(new string[] { "John", "Pot" },
                    11.00f,
                    new string[] { "Pepperoni" },
                    new string[] { "" })]
        [InlineData(new string[] { "John", "Pot" },
                    11.00f,
                    new string[] { "Pepperoni" },
                    new string[] { null },
                    new string[] { "" })]
        // Only invalid pizzas
        [InlineData(new string[] { "John", "Pot" },
                    0.0f,
                    new string[] { },
                    new string[] { null },
                    new string[] { "" })]
        public void CostOfOrderIsCalculatedCorrectly(string[] user, float expectedCost, params string[][] pizzas)
        {
            // Arrange
            Order order1 = new Order(user);
            foreach (string[] pizza in pizzas)
            {
                order1.AddPizzaToOrder(pizza);
            }

            // Act
            float actualCost = order1.Cost;

            // Assert
            Assert.Equal(expectedCost, actualCost);

        }

        // total value cannot exceed $500

        [Fact]
        public void CostOfOrderIsCappedAtFiveHundred()
        {
            // Arrange
            Order order1 = new Order(new string[] { "John", "Pot" });

            Pizza reallyExpensivePizza = new Pizza();
            // Create a $100 pizza ($10 base + 90-$1 toppings)
            for(int i = 0; i < 90; ++i)
            {
                reallyExpensivePizza.AddIngredientsToPizza("Pepperoni");
            }

            // Add six $100 pizzas = $600 total
            for(int i = 0; i < 6; ++i)
            {
                order1.AddPizzaToOrder(reallyExpensivePizza);
            }

            // Act
            float actualCost = order1.Cost;

            // Assert
            Assert.Equal(500, actualCost);
            // Order logic should reject the last pizza
            Assert.Equal(5, order1.PizzaList.Count);
        }
    
        #endregion
    }
}
