using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;
using lib = PizzaStoreLibrary.library;
using db = PizzaStoreData.DataAccess;

namespace PizzaStoreTesting.test.DataAccessTesting
{
    public class UserDataAccessTesting
    {
        #region User Mapping
        [Fact]
        public void UserMapFromDBUserToLibraryUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                    .UseInMemoryDatabase("User_Mapping_Test1").Options;
            // Create a db.User to map to lib.User
            db.User dbUser = new db.User
            {
                FirstName = "John",
                LastName = "Pot"
            };
            // Add the user to the database for validation
            using (var database = new db.PizzaStoreContext(options))
            {
                database.Add(dbUser);
                database.SaveChanges();
            }

            // Act
            // Attempt to map from db.User to lib.User
            lib.User libUser = db.Mapper.Map(dbUser, options);

            // Assert
            Assert.Equal("John", libUser.FirstName);
            Assert.Equal("Pot", libUser.LastName);
            Assert.Equal(1, libUser.Id);

        }

        [Fact]
        public void UserMapFromLibraryUserToDBUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                    .UseInMemoryDatabase("User_Mapping_Test2").Options;
            // Create a lib.User that will be mapped to a db.User
            lib.User libUser = new lib.User("John", "Pot");

            using (var database = new db.PizzaStoreContext(options))
            {
                database.Add(new db.User
                {
                    FirstName = libUser.FirstName,
                    LastName = libUser.LastName
                });
                database.SaveChanges();
                libUser.Id = database.User.First(u => u.FirstName == libUser.FirstName && u.LastName == libUser.LastName).UserId;
            }

            // Act
            // Attempt to map from lib.User to db.User
            db.User dbUser = db.Mapper.Map(libUser, options);

            // Assert
            Assert.Equal("John", dbUser.FirstName);
            Assert.Equal("Pot", dbUser.LastName);

        }

        #endregion

        /*
        #region User Repo Testing
        // This region assumes mapping works correctly
        [Fact]
        public void RepoCreateCorrectlySetsCSharpId()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                .UseInMemoryDatabase("UserMapping_Test").Options;
            var dataBase = new db.PizzaStoreContext(options);
            db.UserRepository userRepo = new db.UserRepository(options);
            lib.User userL = new lib.User("John", "Pot");

            // Act
            userRepo.Create(userL);
            lib.User userFromDb;
            using (var database = new db.PizzaStoreContext(options))
            {
                userFromDb = db.Mapper.Map(database.User.Find(userL.Id), options);
            }
            // Assert
            Assert.True(userFromDb.Id == userL.Id);
        }
        [Fact]
        public void RepoDeleteRemovesUserFromDB()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                .UseInMemoryDatabase("UserMapping_Test").Options;

            lib.User userL1 = new lib.User("John1", "Pot");
            lib.User userL2 = new lib.User("John2", "Pot");
            using (var db = new db.PizzaStoreContext(options))
            {
                db.UserRepository userRepo = new db.UserRepository(options);
                userRepo.Create(userL1);
                userRepo.Create(userL2);
            }

            // Act
            using (var db = new db.PizzaStoreContext(options))
            {
                db.UserRepository userRepo = new db.UserRepository(options);
                // Delete by entity
                userRepo.Delete(userL1);
                
                // Delete by id
                userRepo.Delete(userL2.Id);

                userRepo.SaveChanges();
            }

            // Assert
            using (var db = new db.PizzaStoreContext(options))
            {
                db.User u1 = db.User.Find(userL1.Id);
                db.User u2 = db.User.Find(userL2.Id);
                Assert.True(u1 == null);
                Assert.True(u2 == null);
            }

        }
        [Fact]
        public void UserRepoCanRetrieveUsers()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                .UseInMemoryDatabase("User_Repo_Testing").Options;
            lib.User u1 = new lib.User("John1", "Pot");
            lib.User u2 = new lib.User("John2", "Pot");

            using (var db = new db.PizzaStoreContext(options))
            {
                // Assuming the repo create works so that
                //  I can use the auto set sql identity in c#
                var userRepo = new db.UserRepository(options);
                userRepo.Create(u1);
                userRepo.Create(u2);
                userRepo.SaveChanges();
            }
            // Act
            lib.User retrievedUser1, retrievedUser2;
            using (var db = new db.PizzaStoreContext(options))
            {
                var userRepo = new db.UserRepository(options);
                retrievedUser1 = userRepo.GetById(u1.Id, options);
                retrievedUser2 = userRepo.GetByName(u2.FirstName, u2.LastName, options);
            }

            // Assert
            using (var db = new db.PizzaStoreContext(options))
            {
                db.User dbUser1 = db.User.Find(u1.Id);
                db.User dbUser2 = db.User.Find(u2.Id);

                Assert.NotNull(dbUser1);
                Assert.NotNull(dbUser2);
            }

        }

        [Fact]
        public void UserRepoCanUpdateUserInformation()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<db.PizzaStoreContext>()
                .UseInMemoryDatabase("User_Repo_Testing").Options;
            lib.User libUser1 = new lib.User("John", "Pot");

            using (var db = new db.PizzaStoreContext(options))
            {
                var userRepo = new db.UserRepository(options);
                userRepo.Create(libUser1);
                userRepo.SaveChanges();
            }

            // Act
            using (var db = new db.PizzaStoreContext(options))
            {
                var userRepo = new db.UserRepository(options);
                libUser1.FirstName = "Jim";
                userRepo.Update(libUser1);
                userRepo.SaveChanges();
            }

            // Assert
            using (var db = new db.PizzaStoreContext(options))
            {
                db.User dbUser = db.User.FirstOrDefault(u => u.FirstName == "Jim" && u.LastName == "Pot");
                Assert.NotNull(dbUser);
            }
        }

        #endregion  
    */
    }
}
