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
        [InlineData(new string[] { "Cheese", "Pepperoni", "Olives"},
                    new int[] { 10, 10, 10},
                    "John's Pizzaria",
                    30)]
        // Pass just one ingredient type
        [InlineData(new string[] { "Cheese" },
                    new int[] { 10 },
                    "John's Pizzaria",
                    10)]
        // No ingredients
        [InlineData(new string[] { },
                    new int[] {  },
                    "John's Pizzaria",
                    0)]
        public void LocationHasIngredients(string[] ingredients, int[] inventory, string name, int totalInventory)
        {
            // Arrange
            Location location = new Location(name);
            for(int i = 0; i < ingredients.Length; ++i)
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

        // inventory decreases when orders are accepted
        #region Orders Being Placed

        [Theory]
        [InlineData(new string[] { "Cheese", "Olives" },
                    new int[] { 10, 10 },
                    "John's Pizzaria",
                    8)]
        public void InventoryDepletesAsOrdersAreProcessed(string[] ingredients, int[] inventory, string name, int expectedRemaing)
        {
            // Arrange
            Location location = new Location(name);
            for(int i = 0; i < ingredients.Length; ++i)
            {
                location.StockInventory(new KeyValuePair<string, int>(ingredients[i], inventory[i]));
            }

            // Act
            //location.PlaceOrder(new Order("Cheese", "Olives"));
            int remainingInventory = location.Inventory.Sum(i => i.Value);

            // Assert
            Assert.Equal(expectedRemaing, remainingInventory);

        }

        #endregion


        // rejects orders that cannot be fulfilled with remaining inventory


        // has order history


    }
}
