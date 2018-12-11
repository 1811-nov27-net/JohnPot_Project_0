using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using db = PizzaStoreData.DataAccess;
using lib = PizzaStoreLibrary.library;


namespace PizzaStoreInterface.UI
{
    public class UserMethod
    {
        private readonly DbContextOptions<db.PizzaStoreContext> _options;
        private Repo repo;
        private lib.StoreManager store;

        private Dictionary<Responses, string> MethodResponse;
        private enum Responses
        {
            // Valid responses
            LocationCreated,
            DefaultLocationSet,
            LocationStocked,

            // Invalid responses
            InvalidLocationName,
            LocationExists,
            InvalidUserName,
            InvalidPizzaTopping,
            TooManyPizzas,
            NoInventoryCreated
        }

        private void GenerateResponses()
        {
            // Valid responses
            MethodResponse[Responses.LocationCreated] = "New location was successfully created: ";
            MethodResponse[Responses.DefaultLocationSet] = "Default location successfully set to: ";
            MethodResponse[Responses.LocationStocked] = "Location was successfully stocked!";

            // Invalid responses
            MethodResponse[Responses.LocationExists] = "Location already exists!: ";
            MethodResponse[Responses.InvalidLocationName] = "Invalid Location Name!";
            MethodResponse[Responses.InvalidUserName] = "Invalid User Name!";
            MethodResponse[Responses.InvalidPizzaTopping] = "Invalid Pizza Topping!";
            MethodResponse[Responses.TooManyPizzas] = "You can only have a maximum of 12 pizzas per order!";
            MethodResponse[Responses.NoInventoryCreated] = "No inventory for location/ingredient was found.";
        }

        public string SanitizeInput(string input)
        {
            // Do more here. This is bad.
            return input.ToLower();
        }
        public string test(string input)
        {
            return "You entered: " + input;
        }

        public UserMethod(DbContextOptions<db.PizzaStoreContext> options)
        {
            _options = options;
            repo = new Repo(options);
            MethodResponse = new Dictionary<Responses, string>();
            GenerateResponses();

            store = new lib.StoreManager();
        }


        public string createnewlocation(string name)
        {
            db.Location dbLocation = new db.Location() { Name = name };
            dbLocation.LocationId = db.Mapper.GetLocationIdByName(name, _options);
            // Make sure the location we are creating does not already exist
            if (dbLocation.LocationId != -1)
                return MethodResponse[Responses.LocationExists] + name;

            // The location does not exist. Create and add it to the database
            repo.locationRepository.Create(dbLocation);

            // Add a lib.Location to the store manager for lib use
            store.RegisterNewLocation(db.Mapper.Map(dbLocation, _options));

            repo.SaveAll();

            return MethodResponse[Responses.LocationCreated] + name;

        }
        public string stocklocation(string locationName, string ingredientName, int ingredientCount)
        {
            // Make sure the location exists within the database
            db.Location dbLocation = new db.Location() { Name = locationName };
            dbLocation.LocationId = db.Mapper.GetLocationIdByName(locationName, _options);
            if (dbLocation.LocationId == -1)
                return MethodResponse[Responses.InvalidLocationName] + locationName;
            // Make sure the ingredient exists within the database
            db.Ingredient dbIngredient = new db.Ingredient() { Name = ingredientName };
            dbIngredient = repo.ingredientRepository.GetByName(ingredientName);
            if (dbIngredient == null)
                return MethodResponse[Responses.InvalidPizzaTopping] + ingredientName;

            lib.Location libLocation = store.GetLocationByName(locationName);
            if (libLocation == null)
                return MethodResponse[Responses.InvalidLocationName] + locationName;

            // Stock the libLocation inventory
            libLocation.StockInventory(new KeyValuePair<string, int>(ingredientName, ingredientCount));

            // Stock the database inventory
            db.Inventory dbInventory;
            dbInventory = repo.inventoryRepository.GetById(dbLocation.LocationId, dbIngredient.IngredientId);
            if (dbInventory == null)
            {
                // Location does not have ingredient currently... Add it
                //  the the inventory.
                dbInventory = new db.Inventory();
                dbInventory.LocationId = dbLocation.LocationId;
                dbInventory.IngredientId = dbIngredient.IngredientId;
                dbInventory.Count = ingredientCount;
                repo.inventoryRepository.Create(dbInventory);
                repo.SaveAll();
                return MethodResponse[Responses.LocationStocked];
            }

            // Everything exists on the database already. Just update
            //  the inventory count
            dbInventory.Count += ingredientCount;
            repo.inventoryRepository.Update(dbInventory);
            repo.SaveAll();
            return MethodResponse[Responses.LocationStocked];
        }

        public string setdefaultlocation(string name, db.User user)
        {
            // Make sure the location exists
            db.Location dbLocation = new db.Location() { Name = name };
            dbLocation.LocationId = db.Mapper.GetLocationIdByName(name, _options);
            if (dbLocation.LocationId == -1)
                return MethodResponse[Responses.InvalidLocationName] + name;

            // Assuming the user exists already. It should be a valid
            //  user passed in through code, not user input
            user.DefaultLocationId = dbLocation.LocationId;
            repo.userRepository.Update(user);
            repo.SaveAll();

            return MethodResponse[Responses.DefaultLocationSet] + name;
        }
        

    }
}
