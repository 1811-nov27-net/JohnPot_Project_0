using PizzaStoreLibrary.library;
using System;
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
                    Name.InvalidLocation)]
        // Null location
        [InlineData(null,
                    new string[] { "John", "Pot" },
                    Name.InvalidLocation)]
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
            string actualLocationOrder1 = order1.Location.Name.Location;
            string actualLocationOrder2 = order2.Location.Name.Location;
            
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
        [InlineData(new string[] { "John"},
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

        public void OrderHasCorrectTimeWhenPlaced()
        {

        }

        #endregion

        #region Number of Pizzas order can have
        // can have at most 12 pizzas

        [Theory]
        // Providing valid information. A user and 3 pizzas
        [InlineData(new string[] { "John", "Pot" },
                    3,
                    new string[] { "Peperoni", "Olives" },
                    new string[] { "Cheese", "Olives" },
                    new string[] { "Olives" })]
        // Passing in only one pizza
        [InlineData(new string[] { "John", "Pot" },
                    1,
                    new string[] { "Peperoni", "Olives" })]
        // Too many pizzas (13 total)
        [InlineData(new string[] { "John", "Pot" },
                    12,
                    new string[] { "Peperoni", "Olives" },
                    new string[] { "Peperoni", "Olives" },
                    new string[] { "Peperoni", "Olives" },
                    new string[] { "Peperoni", "Olives" },
                    new string[] { "Peperoni", "Olives" },
                    new string[] { "Peperoni", "Olives" },
                    new string[] { "Peperoni", "Olives" },
                    new string[] { "Peperoni", "Olives" },
                    new string[] { "Peperoni", "Olives" },
                    new string[] { "Peperoni", "Olives" },
                    new string[] { "Peperoni", "Olives" },
                    new string[] { "Peperoni", "Olives" },
                    new string[] { "Peperoni", "Olives" })]
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
            if(pizzas != null)
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

        // total value cannot exceed $500
        
        [Theory]
        [InlineData(new string[] { "John", "Pot" },
                    25,
                    new string[] { "Cheese", "Peperoni" },
                    new string[] { "Olives" })]
        public void CostOfOrderIsCalculatedCorrectly(string[] user, int expectedCost, params string[][] pizzas)
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

        #endregion
    }
}
