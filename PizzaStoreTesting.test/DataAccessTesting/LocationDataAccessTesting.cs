using System;
using Xunit;
using lib = PizzaStoreLibrary.library;
using db = PizzaStoreData.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PizzaStoreTesting.test.DataAccessTesting
{
    public class LocationDataAccessTesting
    {

        #region Location Mapping

        [Fact]
        public void LocationMapFromDBLocationToLibraryLocation()
        {
            // Arrange
            string expectedName1 = "c";
            string expectedName2 = "d";

            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                .UseInMemoryDatabase("Location_Mapping_Test1").Options;

            // Create a couple db.Locations that will be used to map
            //  to lib.Locations
            db.Location dbLocation1 = new db.Location();
            dbLocation1.Name = expectedName1;
            db.Location dbLocation2 = new db.Location();
            dbLocation2.Name = expectedName2;
            // Add the new locations to the db for proper 
            //  validation
            using (var database = new db.PizzaStoreContext(options))
            {
                database.Add(dbLocation1);
                database.Add(dbLocation2);
                database.SaveChanges();
            }

            // Act
            // Attempt to map the db.Locations to lib.Locations
            lib.Location libLocation1 = db.Mapper.Map(dbLocation1, options);
            lib.Location libLocation2 = db.Mapper.Map(dbLocation2, options);

            // Assert
            Assert.Equal(expectedName1, libLocation1.Name);
            Assert.Equal(expectedName2, libLocation2.Name);
        }

        [Fact]
        public void LocationMapFromLibraryLocationToDBLocation()
        {
            // Arrange
            string expectedName1 = "e";

            string expectedName2 = "b";

            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                .UseInMemoryDatabase("Location_Mapping_Test2").Options;

            // Create a couple lib.Locations to be mapped
            lib.Location l1 = new lib.Location(expectedName1);
            lib.Location l2 = new lib.Location(expectedName2);
            // Locations need to be registed to be valid locations
            //  within the database. Use LocationRepo to register
            //  on creation. For testing purposes I will manually
            //  add the locations to the database
            using (var dataBase = new db.PizzaStoreContext(options))
            {
                dataBase.Add(new db.Location { Name = expectedName1 });
                dataBase.Add(new db.Location { Name = expectedName2 });
                dataBase.SaveChanges();
                // Register location will also handle id setting.
                l1.Id = dataBase.Location.FirstOrDefault(l => l.Name == expectedName1).LocationId;
                l2.Id = dataBase.Location.FirstOrDefault(l => l.Name == expectedName2).LocationId;
            }

            // Act
            db.Location dbLocation1 = db.Mapper.Map(l1, options);
            db.Location dbLocation2 = db.Mapper.Map(l2, options);

            // Assert
            Assert.Equal(expectedName1, dbLocation1.Name);
            Assert.Equal(expectedName2, dbLocation2.Name);
        }

        #endregion

        #region Location Repository 

        [Fact]
        public void GetByIdWorks()
        {
            // Arrange 
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                    .UseInMemoryDatabase("Location_Repo_Test_1").Options;

            db.LocationRepository locationRepo = new db.LocationRepository(options);
            db.Location expectedDbLocation;
            int locationId;
            // Add a location to try and retrieve
            using (var database = new db.PizzaStoreContext(options))
            {
                expectedDbLocation = new db.Location { Name = "a" };
                database.Add(expectedDbLocation);
                database.SaveChanges();
                locationId = expectedDbLocation.LocationId;
            }

            // Act
            db.Location dbLocation;
            using (var database = new db.PizzaStoreContext(options))
            {
                dbLocation = locationRepo.GetById(locationId);
            }

            // Assert
            Assert.Equal(expectedDbLocation.Name, dbLocation.Name);
            Assert.Equal(expectedDbLocation.LocationId, dbLocation.LocationId);
        }

        [Fact]
        public void CreateNewDBLocationWorks()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                    .UseInMemoryDatabase("Location_Repo_Test_Create").Options;

            db.LocationRepository locationRepo = new db.LocationRepository(options);
            db.Location expectedDbLocation = new db.Location { Name = "a" };

            // Act
            locationRepo.Create(expectedDbLocation);
            locationRepo.SaveChanges();

            // Assert
            using (var database = new db.PizzaStoreContext(options))
            {
                Assert.NotNull(database.Location.Find(expectedDbLocation.LocationId));
            }
        }
        [Fact]
        public void CreateNewLibraryLocationWorks()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                    .UseInMemoryDatabase("Location_Repo_Test_LibCreate").Options;
            db.LocationRepository locationRepo = new db.LocationRepository(options);
            lib.Location libLocation = new lib.Location("a");

            // Act
            locationRepo.Create(libLocation);
            locationRepo.SaveChanges();

            // Assert
            using (var database = new db.PizzaStoreContext(options))
            {
                Assert.NotNull(database.Location.Find(libLocation.Id));
            }
        }

        [Fact]
        public void DeleteDBLocationWorks()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                    .UseInMemoryDatabase("Location_Repo_Test_Delete").Options;

            db.LocationRepository locationRepo = new db.LocationRepository(options);
            db.Location dbLocation = new db.Location() { Name = "a" };
            locationRepo.Create(dbLocation);
            locationRepo.SaveChanges();
            int locationId = dbLocation.LocationId;

            // Act
            locationRepo.Delete(dbLocation);
            locationRepo.SaveChanges();

            // Assert
            using (var database = new db.PizzaStoreContext(options))
                Assert.Null(database.Location.Find(locationId));

        }
        [Fact]
        public void DeleteLibraryLocationWorks()
        {
            // Arrange 
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                    .UseInMemoryDatabase("Location_Repo_Test_LibDelete").Options;
            db.LocationRepository locationRepo = new db.LocationRepository(options);
            lib.Location libLocation = new lib.Location("a");
            locationRepo.Create(libLocation);
            locationRepo.SaveChanges();
            int locationId = libLocation.Id;

            // Act
            locationRepo.Delete(libLocation);
            locationRepo.SaveChanges();

            // Assert
            using (var database = new db.PizzaStoreContext(options))
                Assert.Null(database.Location.Find(locationId));

        }

        [Fact]
        public void UpdateDBLocationWorks()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                    .UseInMemoryDatabase("Location_Repo_Test_Update").Options;

            db.LocationRepository locationRepo = new db.LocationRepository(options);
            db.Location dbLocation = new db.Location() { Name = "a" };
            locationRepo.Create(dbLocation);
            locationRepo.SaveChanges();

            // Act
            dbLocation.Name = "b";
            locationRepo.Update(dbLocation);
            locationRepo.SaveChanges();

            // Assert
            using (var database = new db.PizzaStoreContext(options))
                Assert.Equal("b", database.Location.Find(dbLocation.LocationId).Name);
        }
        [Fact]
        public void UpdateLibraryLocationWorks()
        {
            // Arrange 
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                    .UseInMemoryDatabase("Location_Repo_Test_LibUpdate").Options;
            db.LocationRepository locationRepository = new db.LocationRepository(options);
            lib.Location libLocation = new lib.Location("a");
            locationRepository.Create(libLocation);
            locationRepository.SaveChanges();

            // Act
            libLocation.Name = "b";
            locationRepository.Update(libLocation);
            locationRepository.SaveChanges();

            // Assert
            using (var database = new db.PizzaStoreContext(options))
                Assert.Equal("b", database.Location.Find(libLocation.Id).Name);
        }


        #endregion

    }
}
