using System;
using Xunit;
using PizzaStoreLibrary.library;

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
                    new string[] { Name.InvalidName, Name.InvalidName })]
        // Empty name
        [InlineData(new string[] { "" },
                    new string[] { Name.InvalidName, Name.InvalidName })]
        // No name provided
        [InlineData(new string[] { },
                    new string[] { Name.InvalidName, Name.InvalidName })]
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
                    null, Name.InvalidLocation)]
        // Pass in empty location name
        [InlineData(new string[] { "John", "Pot" },
                    "", Name.InvalidLocation)]
        public void UserHasDefaultLocationToOrderFromUsingConstructors(string[] fullName, string location, string expected)
        {
            // Arrange
            User user = new User(fullName, location);

            // Assert
            Assert.Equal(expected, user.DefaultLocation.Location);

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
                    Name.InvalidLocation)]
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
            Assert.Equal(expected, user.DefaultLocation.Location);

        }
        #endregion

        /***** cannot place more than one order from the same location within two hours *****/

        /*
        public void UserCannotOrderFromSameLocationWithinTwoHours()
        {
            // Arrange
            User user = new User(fullName, addresses);

            // Act
            user.PlaceOrder();
            user.PlaceOrder();
            // Assert

            // To place an order does the user need to 
            //  know what an Order is? 
            // Store handles orders?
            // To place an order you need to know:
            //  - Where we are ordering from
            //  - What we are ordering
            //  - Who is ording it
            // Should be handled through order testing!

            // SideNote: Delivery?
            //  Do we care where the user is located?
            // No.
        }
        */

    }

}
