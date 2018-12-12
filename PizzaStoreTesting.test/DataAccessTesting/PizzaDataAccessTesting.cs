using System;
using System.Linq;
using lib = PizzaStoreLibrary.library;
using db = PizzaStoreData.DataAccess;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace PizzaStoreTesting.test.DataAccessTesting
{
    public class PizzaDataAccessTesting
    {
        #region Pizza Mapping
        [Fact]
        public void PizzaMapFromLibraryPizzaToDBPizza()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                    .UseInMemoryDatabase("Pizza_Mapping_Test1").Options;

            // Create lib.Pizza to later map to db.Pizza

            lib.Pizza libPizza = new lib.Pizza();
            libPizza.Id = 1;

            using (var database = new db.PizzaStoreContext(options))
            {
                // db.Pizza will require ingredients
                database.Add(new db.Ingredient { Name = "cheese", Cost = 0.50 });
                database.Add(new db.Ingredient { Name = "pepperoni", Cost = 1.50 });

                // Will also require that lib.Pizza exists in the database
                // Adding cheese to pizza
                database.Add(new db.Pizza { PizzaId = 1, IngredientId =  1, Count = 1});
                // Adding pepperoni to pizza
                database.Add(new db.Pizza { PizzaId = 1, IngredientId =  2, Count = 1});

                database.SaveChanges();
                libPizza.AddIngredientsToPizza("cheese", "pepperoni");
            }

            // Act
            // Attempt to map from lib.Pizza to db.Pizza
            List<db.Pizza> dbPizza = db.Mapper.Map(libPizza, options);

            // Assert
            // Pizza has cheese
            Assert.Equal(1, dbPizza[0].IngredientId);
            // Pizza has pepperoni
            Assert.Equal(2, dbPizza[1].IngredientId);

        }

        [Fact]
        public void PizzaMapFromDBPizzaToLibraryPizza()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                    .UseInMemoryDatabase("Pizza_Mapping_Test2").Options;

            List<db.Pizza> dbPizza = new List<db.Pizza>();
            // Now there is currently one complete pizza in the db.Pizza list 
            //  that has one cheese and one pepperoni
            using (var database = new db.PizzaStoreContext(options))
            {
                // Database requires ingredients to validate pizzas properly
                database.Add(new db.Ingredient { Name = "cheese", Cost = 0.50 });
                database.Add(new db.Ingredient { Name = "pepperoni", Cost = 1.50 });

                database.SaveChanges();

                // Add one cheese to pizza
                dbPizza.Add(new db.Pizza {
                    PizzaId = 1,
                    IngredientId = db.Mapper.GetIngredientByName("cheese", options).IngredientId,
                    Count = 1 });
                // Add one pepperoni to pizza
                dbPizza.Add(new db.Pizza {
                    PizzaId = 1,
                    IngredientId = db.Mapper.GetIngredientByName("pepperoni", options).IngredientId,
                    Count = 1 });

                database.Add(dbPizza[0]);
                database.Add(dbPizza[1]);

                database.SaveChanges();
            }

            // Act
            // Attempt to map db.Pizza to lib.Pizza
            lib.Pizza libPizza = db.Mapper.Map(dbPizza, options);


            // Assert
            Assert.Equal("cheese", libPizza.Ingredients[0]);
            Assert.Equal("pepperoni", libPizza.Ingredients[1]);


        }

        #endregion 
    }
}
