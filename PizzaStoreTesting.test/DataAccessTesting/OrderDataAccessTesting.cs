using System;
using Xunit;
using lib = PizzaStoreLibrary.library;
using db = PizzaStoreData.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PizzaStoreTesting.test.DataAccessTesting
{
    public class OrderDataAccessTesting
    {

        #region Order Mapping
        [Fact]
        public void OrderMappingFromDBOrderToLibraryOrder()
        {

            // Arrange
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                    .UseInMemoryDatabase("Order_Mapping_Test_1").Options;

            db.Order dbOrder = new db.Order();

            using (var database = new db.PizzaStoreContext(options))
            {
                // Order needs location
                db.Location dbLocation = new db.Location { Name = "A" };
                database.Add(dbLocation);
                // Order also needs a user
                db.User dbUser = new db.User { FirstName = "John", LastName = "Pot" };
                database.Add(dbUser);
                // Order should probably also have a couple pizzas
                // Need ingredients to make a pizza...
                db.Ingredient dbIngredient1 = new db.Ingredient { Name = "cheese", Cost = 0.50 };
                db.Ingredient dbIngredient2 = new db.Ingredient { Name = "pepperoni", Cost = 0.50 };
                database.Add(dbIngredient1);
                database.Add(dbIngredient2);
                // Adding one cheese and one pepperoni to pizza
                db.Pizza dbPizza1 = new db.Pizza { PizzaId = 1, IngredientId = dbIngredient1.IngredientId, Count = 1 };
                db.Pizza dbPizza2 = new db.Pizza { PizzaId = 1, IngredientId = dbIngredient2.IngredientId, Count = 1 };
                database.Add(dbPizza1);
                database.Add(dbPizza2);
                // Now we can properly set all the Order fields
                dbOrder.LocationId = dbLocation.LocationId;
                dbOrder.UserId = dbUser.UserId;
                dbOrder.TimePlaced = DateTime.Now;
                dbOrder.PizzaId = 1;
                database.Add(dbOrder);
                database.SaveChanges();
            }

            // Act
            // Order has been placed in the db, now try to map it 
            //  to a lib.Order
            List<db.Order> dbOrderList = new List<db.Order>();
            dbOrderList.Add(dbOrder);
            lib.Order libOrder = db.Mapper.Map(dbOrderList, options);

            // Assert
            Assert.Equal(dbOrder.LocationId, libOrder.Location.Id);
            Assert.Equal(dbOrder.UserId, libOrder.User.Id);
            Assert.Equal(dbOrder.TimePlaced, libOrder.TimePlaced);
            foreach (lib.Pizza libPizza in libOrder.PizzaList)
            {
                Assert.Equal(dbOrder.PizzaId, libPizza.Id);
            }
        }
        [Fact]
        public void OrderMappingFromDBOrderToLibraryOrderWithMultiplePizzas()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                    .UseInMemoryDatabase("Order_Mapping_Test_2").Options;

            List<db.Order> dbOrder = new List<db.Order>();

            using (var database = new db.PizzaStoreContext(options))
            {
                // Order needs location
                db.Location dbLocation = new db.Location { Name = "A" };
                database.Add(dbLocation);
                // Order also needs a user
                db.User dbUser = new db.User { FirstName = "John", LastName = "Pot" };
                database.Add(dbUser);
                // Order should probably also have a couple pizzas
                // Need ingredients to make a pizza...
                db.Ingredient dbIngredient1 = new db.Ingredient { Name = "cheese", Cost = 0.50 };
                db.Ingredient dbIngredient2 = new db.Ingredient { Name = "pepperoni", Cost = 0.50 };
                database.Add(dbIngredient1);
                database.Add(dbIngredient2);
                // Adding one cheese and one pepperoni to pizza
                db.Pizza dbPizza1 = new db.Pizza { PizzaId = 1, IngredientId = dbIngredient1.IngredientId, Count = 1 };
                db.Pizza dbPizza2 = new db.Pizza { PizzaId = 1, IngredientId = dbIngredient2.IngredientId, Count = 1 };
                database.Add(dbPizza1);
                database.Add(dbPizza2);
                // Create and add another pizza
                db.Pizza dbPizza3 = new db.Pizza { PizzaId = 2, IngredientId = dbIngredient1.IngredientId, Count = 1 };
                db.Pizza dbPizza4 = new db.Pizza { PizzaId = 2, IngredientId = dbIngredient2.IngredientId, Count = 1 };
                database.Add(dbPizza3);
                database.Add(dbPizza4);

                // Now we can properly set all the Order fields
                dbOrder.Add(new db.Order
                {
                    LocationId = dbLocation.LocationId,
                    UserId = dbUser.UserId,
                    TimePlaced = DateTime.Now,
                    PizzaId = 1
                });
                dbOrder.Add(new db.Order
                {
                    LocationId = dbLocation.LocationId,
                    UserId = dbUser.UserId,
                    TimePlaced = DateTime.Now,
                    PizzaId = 2
                });

                database.Add(dbOrder[0]);
                database.Add(dbOrder[1]);

                database.SaveChanges();
            }

            // Act
            // Order has been placed in the db, now try to map it 
            //  to a lib.Order
            lib.Order libOrder = db.Mapper.Map(dbOrder, options);

            // Assert
            Assert.Equal(dbOrder[0].LocationId, libOrder.Location.Id);
            Assert.Equal(dbOrder[0].UserId, libOrder.User.Id);
            Assert.Equal(dbOrder[0].TimePlaced, libOrder.TimePlaced);
            foreach (lib.Pizza libPizza in libOrder.PizzaList)
            {
                Assert.True(libPizza.Id == 1 || libPizza.Id == 2);
            }

        }
        [Fact]
        public void OrderMapFromLibraryOrderToDBOrderSinglePizza()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                    .UseInMemoryDatabase("Order_Mapping_Test_3").Options;

            lib.Order libOrder = new lib.Order("John", "Pot");
            //libOrder.AddPizzaToOrder("Cheese");
            //libOrder.AddPizzaToOrder("Pepperoni");

            using (var database = new db.PizzaStoreContext(options))
            {
                // Order needs location
                db.Location dbLocation = new db.Location { Name = "A" };
                database.Add(dbLocation);
                // Order also needs a user
                db.User dbUser = new db.User { FirstName = "John", LastName = "Pot" };
                database.Add(dbUser);
                // Order should probably also have a couple pizzas
                // Need ingredients to make a pizza...
                db.Ingredient dbIngredient1 = new db.Ingredient { Name = "cheese", Cost = 0.50 };
                db.Ingredient dbIngredient2 = new db.Ingredient { Name = "pepperoni", Cost = 1.50 };
                database.Add(dbIngredient1);
                database.Add(dbIngredient2);
                // Adding one cheese and one pepperoni to pizza
                db.Pizza dbPizza1 = new db.Pizza { PizzaId = 1, IngredientId = dbIngredient1.IngredientId, Count = 1 };
                db.Pizza dbPizza2 = new db.Pizza { PizzaId = 1, IngredientId = dbIngredient2.IngredientId, Count = 1 };
                List<db.Pizza> dbPizzaList = new List<db.Pizza>();
                dbPizzaList.Add(dbPizza1);
                dbPizzaList.Add(dbPizza2);
                database.Add(dbPizza1);
                database.Add(dbPizza2);

                db.Order dbO = new db.Order
                {
                    LocationId = dbLocation.LocationId,
                    PizzaId = 1,
                    TimePlaced = DateTime.Now,
                    UserId = dbUser.UserId
                };
                database.SaveChanges();

                libOrder.Location = db.Mapper.Map(dbLocation, options);
                libOrder.User = db.Mapper.Map(dbUser, options);
                libOrder.TimePlaced = DateTime.Now;
                libOrder.AddPizzaToOrder(db.Mapper.Map(dbPizzaList, options));
                libOrder.Id = dbO.OrderId;
            }

            // Act
            List<db.Order> dbOrder = db.Mapper.Map(libOrder, options);


            // Assert
            Assert.Equal(libOrder.Id, dbOrder[0].OrderId);
            Assert.Equal(libOrder.Location.Id, dbOrder[0].LocationId);
            Assert.Equal(libOrder.User.Id, dbOrder[0].UserId);
            Assert.Equal(libOrder.TimePlaced, dbOrder[0].TimePlaced);
            Assert.Equal(libOrder.PizzaList[0].Id, dbOrder[0].PizzaId);
        }
        [Fact]
        public void OrderMapFromLibraryOrderToDBOrderMultiplePizzas()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                    .UseInMemoryDatabase("Order_Mapping_Test_4").Options;

            lib.Order libOrder = new lib.Order("John", "Pot");

            using (var database = new db.PizzaStoreContext(options))
            {
                // Order needs location
                db.Location dbLocation = new db.Location { Name = "A" };
                database.Add(dbLocation);
                // Order also needs a user
                db.User dbUser = new db.User { FirstName = "John", LastName = "Pot" };
                database.Add(dbUser);
                // Order should probably also have a couple pizzas
                // Need ingredients to make a pizza...
                db.Ingredient dbIngredient1 = new db.Ingredient { Name = "cheese", Cost = 0.50 };
                db.Ingredient dbIngredient2 = new db.Ingredient { Name = "pepperoni", Cost = 1.50 };
                database.Add(dbIngredient1);
                database.Add(dbIngredient2);
                // Adding two different pizzas to the order
                db.Pizza dbPizza1 = new db.Pizza { PizzaId = 1, IngredientId = dbIngredient1.IngredientId, Count = 1 };
                db.Pizza dbPizza2 = new db.Pizza { PizzaId = 1, IngredientId = dbIngredient2.IngredientId, Count = 1 };
                db.Pizza dbPizza3 = new db.Pizza { PizzaId = 2, IngredientId = dbIngredient1.IngredientId, Count = 1 };
                db.Pizza dbPizza4 = new db.Pizza { PizzaId = 2, IngredientId = dbIngredient2.IngredientId, Count = 1 };
                database.Add(dbPizza1);
                database.Add(dbPizza2);
                database.Add(dbPizza3);
                database.Add(dbPizza4);
                database.SaveChanges();

                List<db.Pizza> dbPizzaList1 = new List<db.Pizza>();
                dbPizzaList1.Add(dbPizza1);
                dbPizzaList1.Add(dbPizza2);
                List<db.Pizza> dbPizzaList2 = new List<db.Pizza>();
                dbPizzaList2.Add(dbPizza3);
                dbPizzaList2.Add(dbPizza4);


                db.Order dbO1 = new db.Order
                {
                    LocationId = dbLocation.LocationId,
                    PizzaId = 1,
                    TimePlaced = DateTime.Now,
                    UserId = dbUser.UserId
                };
                db.Order dbO2 = new db.Order
                {
                    LocationId = dbLocation.LocationId,
                    PizzaId = 2,
                    TimePlaced = DateTime.Now,
                    UserId = dbUser.UserId
                };


                libOrder.Location = db.Mapper.Map(dbLocation, options);
                libOrder.User = db.Mapper.Map(dbUser, options);
                libOrder.TimePlaced = DateTime.Now;
                libOrder.Id = dbO1.OrderId;

                libOrder.AddPizzaToOrder(db.Mapper.Map(dbPizzaList1, options));
                libOrder.AddPizzaToOrder(db.Mapper.Map(dbPizzaList2, options));
            }

            // Act
            List<db.Order> dbOrder = db.Mapper.Map(libOrder, options);


            // Assert
            Assert.Equal(libOrder.Id, dbOrder[0].OrderId);
            Assert.Equal(libOrder.Location.Id, dbOrder[0].LocationId);
            Assert.Equal(libOrder.User.Id, dbOrder[0].UserId);
            Assert.Equal(libOrder.TimePlaced, dbOrder[0].TimePlaced);
            // Check that all the pizzas were mapped correctly
            
            Assert.Equal(libOrder.PizzaList[0].Id, dbOrder[0].PizzaId);
            Assert.Equal(libOrder.PizzaList[0].Id, dbOrder[1].PizzaId);
            Assert.Equal(libOrder.PizzaList[1].Id, dbOrder[2].PizzaId);
            Assert.Equal(libOrder.PizzaList[1].Id, dbOrder[3].PizzaId);
        }


        #endregion
    }
}
